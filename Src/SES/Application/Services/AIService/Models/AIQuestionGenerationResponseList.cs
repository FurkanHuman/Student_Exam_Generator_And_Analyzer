using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Services.AIService.Models;

/// <summary>
/// Wrapper for AI question generation responses (required for OpenAI JSON Schema)
/// OpenAI requires root schema to be an object, not an array
/// </summary>
public sealed class AIQuestionGenerationResponseList
{
    /// <summary>
    /// List of generated questions
    /// </summary>
    [JsonPropertyName("questions")]
    [Required]
    [MinLength(1)]
    public List<AIQuestionGenerationResponse> Questions { get; set; } = [];
}