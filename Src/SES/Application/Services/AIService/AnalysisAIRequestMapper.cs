
using Application.Services.AIService.Models;
using Application.Services.CalculatorFactory;
using Domain.Entities;
using System.Text.RegularExpressions;

namespace Application.Services.AIAnalysis;

public static partial class AnalysisAIRequestMapper
{
    public static AIAnalysisRequest MapToAIRequest(
                                                   int analysisId,
                                                   Analysis analysis,
                                                   AnalysisGeneralExamStatisticsCounters generalStats,
                                                   IList<AnalysisDetailTableDto> detailTables,
                                                   Dictionary<int, double> questionAverages,
                                                   SuccessDistribution successDistribution,
                                                   BenefitAnalysisResultDto benefitAnalysis,
                                                   AdvancedStatisticsData advancedStats
                                                   )
    {
        AIAnalysisRequest request = new()
        {
            AnalysisId = analysisId.ToString(),
            LessonName = analysis.Lesson?.LessonName ?? "Unknown",
            Grade = ExtractGrade(analysis),
            SemesterName = analysis.Semester?.Name ?? "Unknown",
            PassingScore = analysis.Lesson?.PassingScore ?? 50,
            ClassStats = MapClassStatistics(generalStats, detailTables, successDistribution),
            Questions = MapQuestionAnalysis(questionAverages, advancedStats, analysis),
            StudentProfiles = MapAnonymousStudentProfiles(advancedStats),
            Benefits = MapBenefitAnalysis(benefitAnalysis),
            Clusters = MapClusterDistribution(advancedStats)
        };

        return request;
    }

    private static ClassStatistics MapClassStatistics(
        AnalysisGeneralExamStatisticsCounters generalStats,
        IList<AnalysisDetailTableDto> detailTables,
        SuccessDistribution successDistribution)
    {
        List<double> allScores = [.. detailTables
            .SelectMany(t => t.Students)
            .Select(s => (double)s.StudentAnswerScores.Sum(sa => sa.GivenScore))
            .OrderBy(s => s)];

        return new ClassStatistics
        {
            TotalStudents = generalStats.TotalCount,
            AttendeesCount = generalStats.AttendeesCount,
            AbsenteesCount = generalStats.AbsenteesCount,
            PassedCount = generalStats.PassedCount,
            FailedCount = generalStats.FailedCount,
            AverageScore = allScores.Count != 0 ? allScores.Average() : 0,
            MedianScore = allScores.Count != 0 ? Percentile(allScores, 50) : 0,
            StandardDeviation = allScores.Count != 0 ? CalculateStdDev(allScores) : 0,
            MinScore = allScores.Count != 0 ? allScores.Min() : 0,
            MaxScore = allScores.Count != 0 ? allScores.Max() : 0,
            VeryGoodCount = successDistribution.VeryGood,
            GoodCount = successDistribution.Good,
            AverageCount = successDistribution.Average,
            PassCount = successDistribution.Pass,
            FailCount = successDistribution.Fail
        };
    }

    private static List<QuestionAnalysis> MapQuestionAnalysis(
        Dictionary<int, double> questionAverages,
        AdvancedStatisticsData advancedStats,
        Analysis analysis)
    {
        var questionList = new List<QuestionAnalysis>();

        // Tüm exam'lerdeki quiz questionları topla
        var allQuizQuestions = analysis.Exams?
            .SelectMany(selector: e => e.QuizQuestions ?? Enumerable.Empty<Domain.Entities.QuizQuestion>())
            .ToList() ?? [];

        foreach (var (questionOrder, avgScore) in questionAverages.OrderBy(q => q.Key))
        {
            QuestionAnswerStatsDto? stats = advancedStats.QuestionStats.FirstOrDefault(q => q.QuestionOrder == questionOrder);
            if (stats == null) continue;

            double successRate = stats.TotalStudents > 0
                ? (stats.CorrectCount * 100.0) / stats.TotalStudents
                : 0;

            List<string> relatedBenefits = GetRelatedBenefitsForQuestion(questionOrder, allQuizQuestions);

            questionList.Add(new QuestionAnalysis
            {
                QuestionOrder = questionOrder,
                AverageScore = avgScore,
                SuccessRate = successRate,
                CorrectCount = stats.CorrectCount,
                WrongCount = stats.WrongCount,
                BlankCount = stats.BlankCount,
                PartialCount = stats.PartialCount,
                DifficultyLevel = GetDifficultyLevel(successRate),
                RelatedBenefits = relatedBenefits
            });
        }

        return questionList;
    }

    private static List<string> GetRelatedBenefitsForQuestion(
        int questionOrder,
        List<QuizQuestion> allQuizQuestions)
    {

        if (questionOrder <= 0 || questionOrder > allQuizQuestions.Count)
            return [];

        QuizQuestion question = allQuizQuestions[questionOrder - 1];

        return question.Benefits?
            .Select(b => b.BenefitCode ?? "")
            .Where(code => !string.IsNullOrEmpty(code))
            .Distinct()
            .ToList() ?? [];
    }

    private static List<AnonymousStudentProfile> MapAnonymousStudentProfiles(AdvancedStatisticsData advancedStats)
    {
        var profiles = new List<AnonymousStudentProfile>();

        foreach (var perfData in advancedStats.PerformanceData)
        {
            int total = perfData.CorrectCount + perfData.WrongCount + perfData.BlankCount + perfData.PartialCount;

            profiles.Add(new AnonymousStudentProfile
            {
                ProfileId = Guid.NewGuid().ToString(),
                TotalScore = perfData.TotalScore,
                CorrectRate = total > 0 ? (perfData.CorrectCount * 100.0) / total : 0,
                WrongRate = total > 0 ? (perfData.WrongCount * 100.0) / total : 0,
                BlankRate = total > 0 ? (perfData.BlankCount * 100.0) / total : 0,
                PartialRate = total > 0 ? (perfData.PartialCount * 100.0) / total : 0,
                PerformanceCategory = CategorizePerformance(perfData.TotalScore),
                QuestionsSolved = perfData.CorrectCount + perfData.PartialCount + perfData.WrongCount,
                QuestionsSkipped = perfData.BlankCount
            });
        }

        return profiles;
    }

    private static List<BenefitAnalysis> MapBenefitAnalysis(BenefitAnalysisResultDto benefitAnalysis)
    {
        return [.. benefitAnalysis.ClassAverageBenefits.Select(b => new BenefitAnalysis
        {
            BenefitCode = b.BenefitCode,
            Description = b.Description,
            AverageScore = b.Score,
            SuccessRate = b.Percentage,
            QuestionCount = b.TotalQuestions,
            Status = GetBenefitStatus(b.Score)
        })];
    }

    private static ClusterDistribution MapClusterDistribution(AdvancedStatisticsData advancedStats)
    {
        return new ClusterDistribution
        {
            HighPerformanceCount = advancedStats.ClusteringData.Count(c => c.TotalScore >= 85),
            MediumPerformanceCount = advancedStats.ClusteringData.Count(c => c.TotalScore >= 60 && c.TotalScore < 85),
            LowPerformanceCount = advancedStats.ClusteringData.Count(c => c.TotalScore >= 50 && c.TotalScore < 60),
            RiskGroupCount = advancedStats.ClusteringData.Count(c => c.TotalScore < 50)
        };
    }

    private static string GetDifficultyLevel(double successRate)
    {
        return successRate switch
        {
            >= 80 => "Easy",
            >= 50 => "Medium",
            _ => "Hard"
        };
    }

    private static string CategorizePerformance(double totalScore)
    {
        return totalScore switch
        {
            >= 85 => "Highly Successful",
            >= 70 => "Successful",
            >= 60 => "Average",
            >= 50 => "Pass",
            _ => "At Risk"
        };
    }

    private static string GetBenefitStatus(double score)
    {
        return score switch
        {
            >= 3.5 => "Excellent",
            >= 3.0 => "Good",
            >= 2.5 => "Adequate",
            >= 2.0 => "Needs Improvement",
            _ => "Insufficient"
        };
    }

    private static int ExtractGrade(Domain.Entities.Analysis analysis)
    {
        string lessonName = analysis.Lesson?.LessonName ?? "";
        Match match = LessonGradeRegex().Match(lessonName);
        return match.Success ? int.Parse(match.Value) : 0;
    }

    private static double Percentile(List<double> sortedList, double percentile)
    {
        int n = sortedList.Count;
        double position = (n - 1) * percentile / 100.0;
        int lower = (int)Math.Floor(position);
        int upper = (int)Math.Ceiling(position);

        if (lower == upper)
            return sortedList[lower];

        return sortedList[lower] + (position - lower) * (sortedList[upper] - sortedList[lower]);
    }

    private static double CalculateStdDev(List<double> values)
    {
        double avg = values.Average();
        double sumOfSquares = values.Sum(v => Math.Pow(v - avg, 2));
        return Math.Sqrt(sumOfSquares / values.Count);
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex LessonGradeRegex();
}