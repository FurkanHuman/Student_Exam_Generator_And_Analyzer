using Domain.Enums;


namespace Application.Services.AIService.Models;

public class QuestionAIInComingModel
{
    public QuestionType QuestionType { get; set; }
    public string Question { get; set; }=string.Empty;
    public string QuestionBody { get; set; } = string.Empty;
    public ICollection<string> BenefitCodes { get; set; } = [];
    public uint Score { get; set; }
    public uint MaxScore { get; set; }
    public Dictionary<string, bool>? QuestionOption { get; set; } = [];
}