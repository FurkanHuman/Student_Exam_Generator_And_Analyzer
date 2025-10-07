using Application.Features.Lessons.Queries.GetListLessonQueryBySemesterId;
using Application.Features.Semesters.Queries.GetList;
using Application.Services.Analyses;
using Application.Services.Exams;
using Application.Services.Principals;
using Application.Services.Repositories;
using Application.Services.StudentExamAnswers;
using AutoMapper.Execution;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.BgTask;
internal class AnalysisAutomationService : BackgroundService
{
    private const int DelayInDays = 15;

    private readonly IMediator _mediatr;
    private readonly IExamService _examService;
    private readonly IPrincipalService _principalService;
    private readonly IAnalysisRepository _analysisRepository;

    public AnalysisAutomationService(IMediator mediatr, IExamService examService, IPrincipalService principalService, IAnalysisRepository analysisRepository)
    {
        _mediatr = mediatr;
        _examService = examService;
        _principalService = principalService;
        _analysisRepository = analysisRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int? activeSemesterIdTask = await IsWithinSemesterDateRange();


        if (!activeSemesterIdTask.HasValue)
            return;

        int activeSemesterId = activeSemesterIdTask.Value;

        IPaginate<Exam>? eligibleExams = await _examService.GetListAsync(e => e.EvaluationOrigin != EvaluationOrigin.NotEvaluated
                                                                      && e.SemesterId == activeSemesterId,
                                                                      include: e => e.Include(e => e.StudentExamAnswer)
                                                                                     .Include(e => e.Student)
                                                                                     .Include(e => e.StudentClasses)
                                                                                     .Include(e=>e.Teachers)
                                                                                     .Include(e => e.ReferenceBenefit),

                                                                      index: 0,
                                                                      size: int.MaxValue,
                                                                      cancellationToken: stoppingToken);
        if (eligibleExams == null || eligibleExams.Count == 0)
            return;

        IList<Analysis> semesterAnalyses = [];


        IEnumerable<IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam>> groupedByLessonAndClassWithHashCode = eligibleExams.Items.GroupBy(e => (e.LessonId, e.Student.StudentClassId, e.ExamConfigurationStr.GetHashCode()));

        IPaginate<Principal>? principalList = await _principalService.GetListAsync(
            predicate: p => p.SemesterId == activeSemesterId,
            include: p => p.Include(pr => pr.School),
            orderBy: p => p.OrderByDescending(pr => pr.CreatedDate),
            index: 0,
            size: 1,
            cancellationToken: stoppingToken);

        if (principalList == null || principalList.Count == 0)
            return;

        Principal lastPrincipal = principalList.Items.First();

        School school = lastPrincipal.School;

        foreach (IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam> examGroup in groupedByLessonAndClassWithHashCode)
        {
            IList<StudentExamAnswer> studentExamAnswers = [.. examGroup.Select(e => e.StudentExamAnswer).Where(sea => sea.ExamEvaluationStatus != ExamEvaluationStatus.NotEvaluated)];
            IList<Teacher> teachers = [.. examGroup.SelectMany(e => e.Teachers).DistinctBy(t => t.Id)];

            ReferenceBenefit refBenefit = examGroup.First().ReferenceBenefit;

            Analysis analysis = new()
            {
                LessonId = examGroup.Key.LessonId,
                ReferenceBenefitId = refBenefit.Id,
                PrincipalId = lastPrincipal.Id,
                SemesterId = activeSemesterId,
                SchoolId = school.Id,

                School = school,
                ReferenceBenefit = refBenefit,
                Principal = lastPrincipal,
                AIResponse = string.Empty,
                Exams = [.. examGroup],
                StudentExamAnswers = studentExamAnswers,
                Teachers = teachers,

            };

            semesterAnalyses.Add(analysis);
        }

        if (semesterAnalyses.Count == 0)
            return;

        await _analysisRepository.AddRangeAsync(semesterAnalyses,stoppingToken);
    }

    private async Task<int?> IsWithinSemesterDateRange()
    {
        DateOnly _todayDate = DateOnly.FromDateTime(DateTime.Now);
        GetListResponse<GetListSemesterListItemDto> _GetListSemesters = await _mediatr.Send(new GetListSemesterQuery() { PageRequest = new() { PageIndex = 0, PageSize = int.MaxValue } });
        return _GetListSemesters.Items.LastOrDefault(s => s.BeginSemesterDate <= _todayDate && s.EndSemesterDate >= _todayDate)?.Id;
    }
}
