using Application.Features.Semesters.Queries.GetList;
using Application.Services.Exams;
using Application.Services.Principals;
using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.BackroundServices;
internal class AnalysisAutomationService(IMediator mediatr, IExamService examService, IPrincipalService principalService, IAnalysisRepository analysisRepository) : BackgroundService
{
    private const int DelayInDays = 15;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int? activeSemesterIdTask = await IsWithinSemesterDateRange();


        if (!activeSemesterIdTask.HasValue)
            return;

        int activeSemesterId = activeSemesterIdTask.Value;
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        IPaginate<Exam>? eligibleExams = await GetExamsReadyForAnalysis(activeSemesterId, today, stoppingToken);

        if (eligibleExams == null || eligibleExams.Count == 0)
            return;

        IList<Analysis> semesterAnalyses = [];
        IEnumerable<IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam>> groupedByLessonAndClassWithHashCode = GroupEligibleExamsByLessonAndStudentClassAndConfigHash(eligibleExams);
        IPaginate<Principal>? principalList = await GetLatestPrincipalForSemester(activeSemesterId, stoppingToken);

        if (principalList == null || principalList.Count == 0)
            return;

        Principal lastPrincipal = principalList.Items[0];

        School school = lastPrincipal.School;

        foreach (IGrouping<(int LessonId, int StudentClassId, int hashCode), Exam> examGroup in groupedByLessonAndClassWithHashCode)
        {
            IList<StudentExamAnswer> studentExamAnswers = [.. examGroup.Select(e => e.StudentExamAnswer)];

            IList<Teacher> teachers = [.. examGroup.SelectMany(e => e.Teachers)
                                                   .DistinctBy(t => t.Id)];

            ReferenceBenefit refBenefit = examGroup.First().ReferenceBenefit;

            string studentClassName = $"{examGroup.First().Student.StudentClass.ClassAge}/{examGroup.First().Student.StudentClass.ClassBranch}";
            string analysisName = $"Auto - {examGroup.First().Lesson.LessonName} - {studentClassName} - {refBenefit.ReferenceBenefitName} Anlasis"; // note: name is changable

            Analysis analysis = new()
            {
                Name = analysisName,
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

        await analysisRepository.AddRangeAsync(semesterAnalyses, stoppingToken);
    }

    private async Task<IPaginate<Principal>?> GetLatestPrincipalForSemester(int activeSemesterId, CancellationToken stoppingToken)
    {
        return await principalService.GetListAsync(
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
        return await examService.GetListAsync(e => e.SemesterId == activeSemesterId
                                                && e.ExamDate.AddDays(DelayInDays) == today,
                                                include: e => e.Include(e => e.StudentExamAnswer)
                                                               .Include(e => e.Student)
                                                               .Include(e => e.StudentClasses)
                                                               .Include(e => e.Teachers)
                                                               .Include(e => e.ReferenceBenefit),
                                                index: 0,
                                                size: int.MaxValue,
                                                cancellationToken: stoppingToken);
    }

    private async Task<int?> IsWithinSemesterDateRange()
    {
        DateOnly _todayDate = DateOnly.FromDateTime(DateTime.Now);
        GetListResponse<GetListSemesterListItemDto> _GetListSemesters = await mediatr.Send(new GetListSemesterQuery() { PageRequest = new() { PageIndex = 0, PageSize = int.MaxValue } });
        return _GetListSemesters.Items.LastOrDefault(s => s.BeginSemesterDate <= _todayDate && s.EndSemesterDate >= _todayDate)?.Id;
    }
}
