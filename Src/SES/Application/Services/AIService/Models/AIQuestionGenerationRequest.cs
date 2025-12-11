using Domain.Enums;

namespace Application.Services.AIService.Models;

/// <summary>
/// Request model for AI question generation
/// </summary>
public sealed class AIQuestionGenerationRequest
{
    /// <summary>
    /// Language code (ISO 639-2/T format): "TUR", "ENG", "DEU", etc.
    /// </summary>
    public string LanguageCode { get; set; } = string.Empty;

    /// <summary>
    /// Custom instructions from teacher
    /// </summary>
    public string? TeacherPrompt { get; set; }

    /// <summary>
    /// Subject/lesson name for context
    /// </summary>
    public string LessonName { get; set; } = string.Empty;

    /// <summary>
    /// Difficulty level: 1=Easy, 2=Medium, 3=Hard
    /// </summary>
    public byte DifficultyLevel { get; set; } = 2;

    /// <summary>
    /// Number of questions to generate
    /// </summary>
    public int QuestionCount { get; set; } = 1;

    /// <summary>
    /// Type of questions to generate
    /// </summary>
    public QuestionType QuestionType { get; set; }

    /// <summary>
    /// Learning objectives/benefits: Key=BenefitCode, Value=Description
    /// </summary>
    public Dictionary<string, string> LearningObjectives { get; set; } = new();

    /// <summary>
    /// Validates the request model
    /// </summary>
}
