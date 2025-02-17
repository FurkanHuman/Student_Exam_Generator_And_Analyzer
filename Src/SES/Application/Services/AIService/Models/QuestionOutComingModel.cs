using Domain.Enums;

namespace Application.Services.AIService.Models;
public class QuestionAIOutgoingModel
{
    public string Language { get; set; } = string.Empty;
    public string? TeacherPrompt { get; set; } = string.Empty;
    public string LessonName { get; set; } = string.Empty;
    public uint HardLevel { get; set; }
    public uint HowMuchQuestion { get; set; } = 1;
    public QuestionType QuestionType { get; set; }
    public Dictionary<string, string> Benefits { get; set; } = [];
}
