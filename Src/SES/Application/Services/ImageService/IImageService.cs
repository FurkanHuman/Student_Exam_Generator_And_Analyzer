namespace Application.Services.ImageService
{
    public interface IImageService
    {
        Task<Dictionary<string, object>> UploadFileAsync(FileStream fileStream, CancellationToken cancellationToken);
        Task<Dictionary<string, object>> UploadFileAsync(MemoryStream memoryStream, string fileName, CancellationToken cancellationToken);
        Task<Dictionary<string, object>> UploadFileAsync(byte[] fileBytes, string fileName, CancellationToken cancellationToken);
        Task<(byte[] FileBytes, Dictionary<string, object> Meta)> DownloadFileAsync(string id, CancellationToken cancellationToken);
        Task<Dictionary<string, object>> DeleteFileAsync(string id, CancellationToken cancellationToken);
        Task<Dictionary<string, object>> GetFileMetaAsync(string id, CancellationToken cancellationToken);
        Task<Dictionary<string, object>> GetHealthAsync(CancellationToken cancellationToken);
    }
}
