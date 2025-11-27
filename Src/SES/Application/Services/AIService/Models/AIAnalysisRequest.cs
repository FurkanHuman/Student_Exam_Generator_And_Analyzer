namespace Application.Services.AIService.Models;

public class AIAnalysisRequest
{
    private static readonly ClassStatistics classStatistics = new();

    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();
    public string LessonName { get; set; } = string.Empty;
    public string SemesterName { get; set; } = string.Empty;
    public int PassingScore { get; set; }
    public ClassStatistics ClassStats { get; set; } = classStatistics;
    public List<QuestionAnalysis> Questions { get; set; } = [];
    public List<AnonymousStudentProfile> StudentProfiles { get; set; } = [];
    public List<BenefitAnalysis> Benefits { get; set; } = [];
    public ClusterDistribution Clusters { get; set; } = new();
    public int Grade { get; internal set; }
}

public class ClassStatistics
{
    public int TotalStudents { get; set; }
    public int AttendeesCount { get; set; }
    public int AbsenteesCount { get; set; }
    public int PassedCount { get; set; }
    public int FailedCount { get; set; }
    public double AverageScore { get; set; }
    public double MedianScore { get; set; }
    public double StandardDeviation { get; set; }
    public double MinScore { get; set; }
    public double MaxScore { get; set; }
    public int VeryGoodCount { get; set; }
    public int GoodCount { get; set; }
    public int AverageCount { get; set; }
    public int PassCount { get; set; }
    public int FailCount { get; set; }
}

public class QuestionAnalysis
{
    public int QuestionOrder { get; set; }
    public double AverageScore { get; set; }
    public double SuccessRate { get; set; }
    public int CorrectCount { get; set; }
    public int WrongCount { get; set; }
    public int BlankCount { get; set; }
    public int PartialCount { get; set; }
    public string DifficultyLevel { get; set; } = string.Empty;
    public List<string> RelatedBenefits { get; set; } = [];
}

public class AnonymousStudentProfile
{
    public string ProfileId { get; set; } = Guid.NewGuid().ToString();
    public double TotalScore { get; set; }
    public double CorrectRate { get; set; }
    public double WrongRate { get; set; }
    public double BlankRate { get; set; }
    public double PartialRate { get; set; }
    public string PerformanceCategory { get; set; } = string.Empty;
    public int QuestionsSolved { get; set; }
    public int QuestionsSkipped { get; set; }
}

public class BenefitAnalysis
{
    public string BenefitCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double AverageScore { get; set; }
    public double SuccessRate { get; set; }
    public int QuestionCount { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class ClusterDistribution
{
    public int HighPerformanceCount { get; set; }
    public int MediumPerformanceCount { get; set; }
    public int LowPerformanceCount { get; set; }
    public int RiskGroupCount { get; set; }
}