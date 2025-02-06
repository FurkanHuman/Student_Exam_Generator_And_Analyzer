namespace BlazorWebUI.Client.Pages.Styles.QQ;

public class FileData
    {   public string FileExtension { get; set; } = "";
        public string ContentType { get; set; } = "";
        public long FileSize { get; set; }
        public MemoryStream MemoryStream { get; set; } = new();
    }