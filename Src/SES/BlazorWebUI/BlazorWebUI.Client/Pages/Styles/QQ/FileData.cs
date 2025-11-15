namespace BlazorWebUI.Client.Pages.Styles.QQ;

public class FileData
{
    public string FileName { get; set; } = "";
    public string FileExtension { get; set; } = "";
    public string? Id { get; set; } // Reverse Id from ZeroFile adapter. normally used the Url property.
    public string ContentType { get; set; } = "";
    public long FileSize { get; set; }
    public MemoryStream MemoryStream { get; set; } = new();
}