using Domain.Enums;

namespace Application.Services.AIService.Models;

/// <summary>
/// Response model from AI question generation
/// </summary>
public sealed class AIQuestionGenerationResponse
{
    /// <summary>
    /// Type of question
    /// </summary>
    public QuestionType QuestionType { get; set; }

    /// <summary>
    /// Main question text
    /// </summary>
    public string Question { get; set; } = string.Empty;

    /// <summary>
    /// Additional context or passage (optional)
    /// </summary>
    public string? QuestionBody { get; set; }

    /// <summary>
    /// Related benefit/objective codes
    /// </summary>
    public ICollection<string> BenefitCodes { get; set; } = [];

    /// <summary>
    /// Minimum score for this question
    /// </summary>
    public int MinimumScore { get; set; }

    /// <summary>
    /// Maximum possible score
    /// </summary>
    public int MaximumScore { get; set; }

    /// <summary>
    /// Question options: Key=OptionText, Value=IsCorrect
    /// </summary>
    public Dictionary<string, bool>? Options { get; set; }
}
