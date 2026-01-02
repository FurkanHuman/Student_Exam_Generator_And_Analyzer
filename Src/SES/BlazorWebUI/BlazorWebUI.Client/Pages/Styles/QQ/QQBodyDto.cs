namespace BlazorWebUI.Client.Pages.Styles.QQ;

public class QQBodyDto
{
    public int Id { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string? Stem { get; set; }
    public byte SelectedQType { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public bool IsAIGenerated { get; set; } = false;
    public int? PreviousQuestionId { get; set; }
    public FileData? FileData { get; set; }
    public IDictionary<int, string> SelectedBenefits { get; set; } = new Dictionary<int, string>();
    public ICollection<QuestionOptionDto> QuestionOptions { get; set; } = [];
}
