using Application.Features.Semesters.Queries.GetList;
using Application.Services.Exams;
using Application.Services.Principals;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.BackroundServices;

internal class AnalysisAutomationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AnalysisAutomationService> _logger;
    private readonly IConfiguration _configuration;

    private IMediator _mediatr;
    private IExamService _examService;
    private IPrincipalService _principalService;
    private IAnalysisRepository _analysisRepository;
    
    public AnalysisAutomationService(IServiceScopeFactory serviceScopeFactory, ILogger<AnalysisAutomationService> logger, IConfiguration configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        bool enabled = _configuration.GetValue<bool>("BackgroundServices:AnalysisAutomation:Enabled");

        if (!enabled)
        {
            _logger.LogInformation("Analysis automation service is disabled");
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        _mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
        _examService = scope.ServiceProvider.GetRequiredService<IExamService>();
        _principalService = scope.ServiceProvider.GetRequiredService<IPrincipalService>();
        _analysisRepository = scope.ServiceProvider.GetRequiredService<IAnalysisRepository>();

        _logger.LogInformation("Analysis automation service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            int intervalMinutes = _configuration.GetValue<int>("BackgroundServices:AnalysisAutomation:IntervalMinutes");

            await ProcessAnalysisAutomation(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
        }

        _logger.LogInformation("Analysis automation service stopped");
    }

    private async Task ProcessAnalysisAutomation(CancellationToken stoppingToken)
    {
        int? activeSemesterId = await GetActiveSemesterId();

        if (!activeSemesterId.HasValue)
        {
            _logger.LogInformation("No active semester found");
            return;
        }

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        IPaginate<Exam>? eligibleExams = await GetExamsReadyForAnalysis(activeSemesterId.Value, today, stoppingToken);

        if (eligibleExams == null || eligibleExams.Count == 0)
        {
            _logger.LogInformation("No eligible exams found for analysis");
            return;
        }

        _logger.LogInformation("Found {Count} eligible exams for analysis", eligibleExams.Count);

        IPaginate<Principal>? principalList = await GetLatestPrincipalForSemester(activeSemesterId.Value, stoppingToken);

        if (principalList == null || principalList.Count == 0)
        {
            _logger.LogWarning("No principal found for active semester");
            return;
        }

        Principal lastPrincipal = principalList.Items[0];
        School school = lastPrincipal.School;

        IEnumerable<IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam>> groupedExams =
            GroupEligibleExamsByLessonAndStudentClassAndConfigHash(eligibleExams);

        List<Analysis> semesterAnalyses = CreateAnalysesFromGroupedExams(
            groupedExams,
            activeSemesterId.Value,
            lastPrincipal,
            school);

        if (semesterAnalyses.Count == 0)
        {
            _logger.LogInformation("No analyses to create");
            return;
        }

        await _analysisRepository.AddRangeAsync(semesterAnalyses, stoppingToken);
        _logger.LogInformation("Successfully created {Count} analyses", semesterAnalyses.Count);
    }

    private List<Analysis> CreateAnalysesFromGroupedExams(
        IEnumerable<IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam>> groupedExams,
        int semesterId,
        Principal principal,
        School school)
    {
        List<Analysis> analyses = [];

        foreach (IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam> examGroup in groupedExams)
        {
            IList<StudentExamAnswer> studentExamAnswers = examGroup.Select(e => e.StudentExamAnswer).ToList();
            IList<Teacher> teachers = [.. examGroup.SelectMany(e => e.Teachers).DistinctBy(t => t.Id)];
            ReferenceBenefit refBenefit = examGroup.First().ReferenceBenefit;

            string studentClassName = $"{examGroup.First().Student.StudentClass.ClassAge}/{examGroup.First().Student.StudentClass.ClassBranch}";
            string analysisName = $"Auto - {examGroup.First().Lesson.LessonName} - {studentClassName} - {refBenefit.ReferenceBenefitName} Analysis";

            Analysis analysis = new()
            {
                Name = analysisName,
                LessonId = examGroup.Key.LessonId,
                ReferenceBenefitId = refBenefit.Id,
                PrincipalId = principal.Id,
                SemesterId = semesterId,
                SchoolId = school.Id,
                School = school,
                ReferenceBenefit = refBenefit,
                Principal = principal,
                AIResponse = string.Empty,
                Exams = [.. examGroup],
                StudentExamAnswers = studentExamAnswers,
                Teachers = teachers,
            };

            analyses.Add(analysis);
        }

        return analyses;
    }

    private async Task<IPaginate<Principal>?> GetLatestPrincipalForSemester(int activeSemesterId, CancellationToken stoppingToken)
    {
        return await _principalService.GetListAsync(
            predicate: p => p.SemesterId == activeSemesterId,
            include: p => p.Include(pr => pr.School),
            orderBy: p => p.OrderByDescending(pr => pr.CreatedDate),
            index: 0,
            size: 1,
            cancellationToken: stoppingToken);
    }

    private static IEnumerable<IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam>> GroupEligibleExamsByLessonAndStudentClassAndConfigHash(IPaginate<Exam> eligibleExams)
    {
        return eligibleExams.Items.GroupBy(e => (e.LessonId, e.Student.StudentClassId, e.ExamConfigurationStr.GetHashCode()));
    }

    private async Task<IPaginate<Exam>?> GetExamsReadyForAnalysis(int activeSemesterId, DateOnly today, CancellationToken stoppingToken)
    {
        int examDelayDays = _configuration.GetValue<int>("BackgroundServices:AnalysisAutomation:ExamDelayDays");

        return await _examService.GetListAsync(
            predicate: e => e.SemesterId == activeSemesterId && e.ExamDate.AddDays(examDelayDays) == today,
            include: e => e.Include(e => e.StudentExamAnswer)
                           .Include(e => e.Student).ThenInclude(s => s.StudentClass)
                           .Include(e => e.StudentClasses)
                           .Include(e => e.Teachers)
                           .Include(e => e.ReferenceBenefit)
                           .Include(e => e.Lesson),
            index: 0,
            size: int.MaxValue,
            cancellationToken: stoppingToken);
    }

    private async Task<int?> GetActiveSemesterId()
    {
        DateOnly todayDate = DateOnly.FromDateTime(DateTime.Now);
        GetListResponse<GetListSemesterListItemDto> getListSemesters = await _mediatr.Send(
            new GetListSemesterQuery()
            {
                PageRequest = new() { PageIndex = 0, PageSize = int.MaxValue }
            });

        return getListSemesters.Items
            .LastOrDefault(s => s.BeginSemesterDate <= todayDate && s.EndSemesterDate >= todayDate)?.Id;
    }
}