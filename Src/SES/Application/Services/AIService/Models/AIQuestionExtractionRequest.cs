using Domain.Enums;

namespace Application.Services.AIService.Models;

/// <summary>
/// Request model for extracting questions from documents using AI
/// </summary>
public sealed class AIQuestionExtractionRequest
{
    /// <summary>
    /// Base64 encoded document content
    /// </summary>
    public string DocumentContent { get; set; } = string.Empty;

    /// <summary>
    /// Document file name with extension
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the document (application/pdf, application/vnd.openxmlformats-officedocument.wordprocessingml.document, etc.)
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// Language code (ISO 639-2/T format): "TUR", "ENG", "DEU", etc.
    /// </summary>
    public string LanguageCode { get; set; } = string.Empty;

    /// <summary>
    /// Additional instructions for AI about how to extract questions
    /// </summary>
    public string? ExtractionPrompt { get; set; }

    /// <summary>
    /// Subject/lesson name for context
    /// </summary>
    public string LessonName { get; set; } = string.Empty;

    /// <summary>
    /// Expected difficulty level: 1=Easy, 2=Medium, 3=Hard
    /// </summary>
    public byte DifficultyLevel { get; set; } = 2;

    /// <summary>
    /// Expected question type (if null, AI will detect automatically)
    /// </summary>
    public QuestionType? ExpectedQuestionType { get; set; }

    /// <summary>
    /// Learning objectives/benefits: Key=BenefitCode, Value=Description
    /// </summary>
    public Dictionary<string, string> LearningObjectives { get; set; } = new();

    /// <summary>
    /// Maximum number of questions to extract (0 = no limit)
    /// </summary>
    public int MaxQuestions { get; set; } = 0;
}