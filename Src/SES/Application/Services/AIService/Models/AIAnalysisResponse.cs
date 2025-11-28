namespace Application.Services.AIService.Models;

public class AIAnalysisResponse
{
    public string AnalysisId { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public string AIModelVersion { get; set; } = string.Empty; 
    public GeneralAssessment GeneralEvaluation { get; set; } = new();
    public List<QuestionInsight> QuestionInsights { get; set; } = [];
    public List<BenefitInsight> BenefitInsights { get; set; } = [];
    public BehaviorAnalysis StudentBehavior { get; set; } = new();
    public TeachingRecommendations Recommendations { get; set; } = new();
    public StrengthsWeaknesses Analysis { get; set; } = new();
    public List<InterventionStrategy> Interventions { get; set; } = [];
}

public class GeneralAssessment
{
    public string OverallPerformance { get; set; } = string.Empty; 
    public string PerformanceLevel { get; set; } = string.Empty; 
    public double ConfidenceScore { get; set; } 
    public string Summary { get; set; } = string.Empty; 
    public List<string> KeyFindings { get; set; } = []; 
    public string ComparisonToNorm { get; set; } = string.Empty; 
}

public class QuestionInsight
{
    public int QuestionOrder { get; set; }
    public string DifficultyAssessment { get; set; } = string.Empty;
    public string PerformanceAnalysis { get; set; } = string.Empty;
    public List<string> CommonMistakes { get; set; } = [];
    public List<string> ImprovementSuggestions { get; set; } = [];
    public bool RequiresReview { get; set; }
    public string ReviewReason { get; set; } = string.Empty;
}

public class BenefitInsight
{
    public string BenefitCode { get; set; } = string.Empty;
    public string MasteryLevel { get; set; } = string.Empty; 
    public string Analysis { get; set; } = string.Empty;
    public List<string> TeachingStrategies { get; set; } = []; 
    public int PriorityLevel { get; set; } 
}

public class BehaviorAnalysis
{
    public string OverallBehaviorPattern { get; set; } = string.Empty;
    public List<string> ObservedPatterns { get; set; } = [];
    public StrategyAnalysis StrategicBehavior { get; set; } = new();
    public RiskFactors RiskIndicators { get; set; } = new();
    public string EngagementLevel { get; set; } = string.Empty; 
}

public class StrategyAnalysis
{
    public int StrategicStudentCount { get; set; }
    public string StrategyDescription { get; set; } = string.Empty;
    public bool IsEffective { get; set; }
    public string Recommendation { get; set; } = string.Empty;
}

public class RiskFactors
{
    public List<string> IdentifiedRisks { get; set; } = [];
    public int StudentsAtRisk { get; set; }
    public string SeverityLevel { get; set; } = string.Empty; 
    public List<string> EarlyWarningSignals { get; set; } = [];
}

public class TeachingRecommendations
{
    public List<string> ImmediateActions { get; set; } = []; 
    public List<string> ShortTermGoals { get; set; } = []; 
    public List<string> LongTermStrategies { get; set; } = []; 
    public List<string> DifferentiationSuggestions { get; set; } = []; 
    public List<string> AssessmentRecommendations { get; set; } = []; 
    public List<string> ResourceSuggestions { get; set; } = []; 
}

public class StrengthsWeaknesses
{
    public List<string> Strengths { get; set; } = [];
    public List<string> Weaknesses { get; set; } = [];
    public List<string> Opportunities { get; set; } = []; 
    public List<string> Threats { get; set; } = []; 
}

public class InterventionStrategy
{
    public string TargetGroup { get; set; } = string.Empty; 
    public int AffectedStudentCount { get; set; }
    public List<string> SpecificInterventions { get; set; } = [];
    public string Timeline { get; set; } = string.Empty; 
    public List<string> SuccessIndicators { get; set; } = []; 
    public string ResponsibleParty { get; set; } = string.Empty; 
}
