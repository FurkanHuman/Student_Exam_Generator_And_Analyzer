using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

public class AnalysisAdvancedStatisticsCalculator : AnalysisCalculatorFactory<AdvancedStatisticsData>
{
    private readonly IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> _detailCalculator;
    private readonly IAnalysisCalculatorFactory<Dictionary<int, double>> _averagesCalculator;

    public AnalysisAdvancedStatisticsCalculator(
        IAnalysisService analysisService,
        IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> detailCalculator,
        IAnalysisCalculatorFactory<Dictionary<int, double>> averagesCalculator)
        : base(analysisService)
    {
        _detailCalculator = detailCalculator;
        _averagesCalculator = averagesCalculator;
    }

    public override async Task<AdvancedStatisticsData> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        var detailTables = await _detailCalculator.CalculateAsync(analysisId, cancellationToken);
        var questionAverages = await _averagesCalculator.CalculateAsync(analysisId, cancellationToken);

        var result = new AdvancedStatisticsData
        {
            QuestionAverages = questionAverages
        };

        var allStudents = detailTables.SelectMany(t => t.Students).ToList();

        result.StudentScores = allStudents.Select(s => new StudentScoreDto
        {
            StudentId = s.Id,
            StudentName = $"{s.Name} {s.Surname}",
            TotalScore = s.StudentAnswerScores.Sum(sa => sa.GivenScore)
        }).ToList();

        result.QuestionStats = CalculateQuestionStats(detailTables);

        result.ClusteringData = CalculateClusteringData(allStudents);

        result.PerformanceData = CalculatePerformanceData(allStudents);

        result.ParallelData = CalculateParallelData(allStudents);

        return result;
    }

    private List<QuestionAnswerStatsDto> CalculateQuestionStats(IList<AnalysisDetailTableDto> tables)
    {
        var allQuestions = tables
            .SelectMany(t => t.Students)
            .SelectMany(s => s.StudentAnswerScores)
            .Select(sa => sa.QuestionId)
            .Distinct()
            .OrderBy(q => q)
            .ToList();

        return allQuestions.Select((qId, index) =>
        {
            var answersForQuestion = tables
                .SelectMany(t => t.Students)
                .SelectMany(s => s.StudentAnswerScores)
                .Where(sa => sa.QuestionId == qId)
                .ToList();

            return new QuestionAnswerStatsDto
            {
                QuestionOrder = index + 1,
                CorrectCount = answersForQuestion.Count(a => a.GivenScore >= 7),
                WrongCount = answersForQuestion.Count(a => a.GivenScore > 0 && a.GivenScore < 7),
                BlankCount = answersForQuestion.Count(a => a.GivenScore == 0),
                PartialCount = answersForQuestion.Count(a => a.GivenScore >= 4 && a.GivenScore < 7),
                TotalStudents = answersForQuestion.Count
            };
        }).ToList();
    }

    private List<StudentPerformanceDto> CalculateClusteringData(List<StudentTableDto> students)
    {
        return students.Select(s =>
        {
            int total = s.StudentAnswerScores.Count;
            int blankCount = s.StudentAnswerScores.Count(sa => sa.GivenScore == 0);
            int correctCount = s.StudentAnswerScores.Count(sa => sa.GivenScore >= 7);
            int wrongCount = total - blankCount - correctCount;

            return new StudentPerformanceDto
            {
                StudentId = s.Id,
                StudentName = $"{s.Name} {s.Surname}",
                TotalScore = s.StudentAnswerScores.Sum(sa => sa.GivenScore),
                BlankRate = total > 0 ? (blankCount * 100.0 / total) : 0,
                WrongRate = total > 0 ? (wrongCount * 100.0 / total) : 0,
                CorrectRate = total > 0 ? (correctCount * 100.0 / total) : 0
            };
        }).ToList();
    }

    private List<StudentPerformanceDetailDto> CalculatePerformanceData(List<StudentTableDto> students)
    {
        return students.Select(s =>
        {
            int total = s.StudentAnswerScores.Count;
            int correctCount = s.StudentAnswerScores.Count(sa => sa.GivenScore >= 7);
            int wrongCount = s.StudentAnswerScores.Count(sa => sa.GivenScore > 0 && sa.GivenScore < 7);
            int blankCount = s.StudentAnswerScores.Count(sa => sa.GivenScore == 0);
            int partialCount = s.StudentAnswerScores.Count(sa => sa.GivenScore >= 4 && sa.GivenScore < 7);

            return new StudentPerformanceDetailDto
            {
                StudentId = s.Id,
                StudentName = $"{s.Name} {s.Surname}",
                TotalScore = s.StudentAnswerScores.Sum(sa => sa.GivenScore),
                CorrectCount = correctCount,
                WrongCount = wrongCount,
                BlankCount = blankCount,
                PartialCount = partialCount
            };
        }).ToList();
    }

    private List<MultiDimensionalDataDto> CalculateParallelData(List<StudentTableDto> students)
    {
        return students.Select(s =>
        {
            int total = s.StudentAnswerScores.Count;
            int correctCount = s.StudentAnswerScores.Count(sa => sa.GivenScore >= 7);
            int wrongCount = s.StudentAnswerScores.Count(sa => sa.GivenScore > 0 && sa.GivenScore < 7);
            int blankCount = s.StudentAnswerScores.Count(sa => sa.GivenScore == 0);
            int partialCount = s.StudentAnswerScores.Count(sa => sa.GivenScore >= 4 && sa.GivenScore < 7);

            return new MultiDimensionalDataDto
            {
                StudentName = $"{s.Name} {s.Surname}",
                TotalScore = s.StudentAnswerScores.Sum(sa => sa.GivenScore),
                CorrectRate = total > 0 ? (correctCount * 100.0 / total) : 0,
                WrongRate = total > 0 ? (wrongCount * 100.0 / total) : 0,
                BlankRate = total > 0 ? (blankCount * 100.0 / total) : 0,
                PartialRate = total > 0 ? (partialCount * 100.0 / total) : 0
            };
        }).ToList();
    }
}

public class AdvancedStatisticsData
{
    public List<StudentScoreDto> StudentScores { get; set; } = new();
    public Dictionary<int, double> QuestionAverages { get; set; } = new();
    public List<QuestionAnswerStatsDto> QuestionStats { get; set; } = new();
    public List<StudentPerformanceDto> ClusteringData { get; set; } = new();
    public List<StudentPerformanceDetailDto> PerformanceData { get; set; } = new();
    public List<MultiDimensionalDataDto> ParallelData { get; set; } = new();
}

public class StudentScoreDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int TotalScore { get; set; }
}

public class QuestionAnswerStatsDto
{
    public int QuestionOrder { get; set; }
    public int CorrectCount { get; set; }
    public int WrongCount { get; set; }
    public int BlankCount { get; set; }
    public int PartialCount { get; set; }
    public int TotalStudents { get; set; }
}

public class StudentPerformanceDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public double TotalScore { get; set; }
    public double BlankRate { get; set; }
    public double WrongRate { get; set; }
    public double CorrectRate { get; set; }
}

public class StudentPerformanceDetailDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public double TotalScore { get; set; }
    public int CorrectCount { get; set; }
    public int WrongCount { get; set; }
    public int BlankCount { get; set; }
    public int PartialCount { get; set; }
}

public class MultiDimensionalDataDto
{
    public string StudentName { get; set; } = string.Empty;
    public double TotalScore { get; set; }
    public double CorrectRate { get; set; }
    public double WrongRate { get; set; }
    public double BlankRate { get; set; }
    public double PartialRate { get; set; }
}