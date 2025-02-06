namespace BlazorWebUI.Client.Pages.Styles.QQ;

public class QQBodyDto
{
    public string QuestionBody { get; set; } = string.Empty;
    public string? Question { get; set; }
    public byte SelectedQType { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public FileData? FileData { get; set; }
    public ICollection<int> SelectedBenefitIds { get; set; } = [];
    public ICollection<QuestionOptionDto> QuestionOptions { get; set; } = [];
}
