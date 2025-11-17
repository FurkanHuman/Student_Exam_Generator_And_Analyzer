
using Application.Services.Analyses;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CalculatorFactory;

public class AnalysisGeneralStatisticsCounterCalculator
{
    private readonly IAnalysisService _analysisService;
    private const int _passScore = 50;

    public AnalysisGeneralStatisticsCounterCalculator(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    public async Task<AnalysisGeneralExamStatisticsCounters> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        Analysis? analsis = await _analysisService.GetAsync(predicate: a => a.Id == analysisId,
                                                            include: ai => ai.Include(a => a.StudentExamAnswers)
                                                                             .ThenInclude(sea => sea.StudentAnswers)
                                                                             .Include(a => a.Exams),
                                                            cancellationToken: cancellationToken);

        if (analsis == null)
            return new AnalysisGeneralExamStatisticsCounters();

        if (DifrenceExamAndStudentExamAnswers(analsis) < 0)
            throw new InvalidOperationException("Student exam answers count cannot be greater than exams count. Data Integrity is broken");


        return new AnalysisGeneralExamStatisticsCounters()
        {
            AttendeesCount = AttendeesCount(analsis),
            AbsenteesCount = AbsenteesCount(analsis),
            ExcusedCount = ExcusedCount(analsis),
            FailedCount = FailedCount(analsis),
            PassedCount = PassedCount(analsis),
            NotReadingCount = NotReadingCount(analsis),
            TotalCount = TotalCount(analsis)
        };
    }

    private static int DifrenceExamAndStudentExamAnswers(Analysis analysis)
    {
        int examCount = analysis.Exams?.Count ?? 0;
        int studentExamAnswerCount = analysis.StudentExamAnswers?.Count ?? 0;
        return examCount - studentExamAnswerCount;
    }

    private static int AttendeesCount(Analysis analysis)
    {
        return analysis.StudentExamAnswers.Count(sea =>
                                                 sea.StudentAnswers != null &&
                                                 sea.StudentAnswers.Any() &&
                                                 sea.ExamEvaluationStatus is not (
                                                     ExamEvaluationStatus.Excused or
                                                     ExamEvaluationStatus.ExcusedWithReport
                                                 )
        );
    }

    private static int ExcusedCount(Analysis analysis)
    {
        return analysis.StudentExamAnswers.Count(sea =>
                                                 sea.ExamEvaluationStatus is
                                                 ExamEvaluationStatus.Excused or
                                                 ExamEvaluationStatus.ExcusedWithReport
        );
    }
    private static int AbsenteesCount(Analysis analysis)
    {
        return analysis.StudentExamAnswers.Count(sea =>
                                                (sea.StudentAnswers == null || !sea.StudentAnswers.Any()) &&
                                                 sea.ExamEvaluationStatus is
                                                     ExamEvaluationStatus.NotEvaluated or
                                                     ExamEvaluationStatus.Excused or
                                                     ExamEvaluationStatus.ExcusedWithReport
        );
    }


    private static int PassedCount(Analysis analysis)
    {
        return analysis.StudentExamAnswers.Count(sea => sea.StudentAnswers.Sum(sa => sa.GivenScore) >= _passScore);
    }

    private static int FailedCount(Analysis analysis)
    {
        return analysis.StudentExamAnswers.Count(sea => sea.StudentAnswers.Sum(sa => sa.GivenScore) < _passScore);
    }
    private static int TotalCount(Analysis analysis)
    {
        return analysis.StudentExamAnswers?.Count ?? 0;
    }

    private static int NotReadingCount(Analysis analysis)
    {
        return analysis.StudentExamAnswers.Count(sea =>
            sea.StudentAnswers == null || !sea.StudentAnswers.Any() &&
            sea.ExamEvaluationStatus == ExamEvaluationStatus.NotEvaluated
        );
    }
}

public record AnalysisGeneralExamStatisticsCounters
{
    public int AttendeesCount { get; set; }
    public int AbsenteesCount { get; set; }
    public int ExcusedCount { get; set; }
    public int PassedCount { get; set; }
    public int FailedCount { get; set; }
    public int TotalCount { get; set; }
    public int NotReadingCount { get; set; }
}
