
namespace Infrastructure.Adapters.AIService;

internal static class RequestRecorder
{
    private const string LogPath = "Ailog";
    internal static async Task RecordAsync(string request, string response, string modelName, CancellationToken cancellationToken)
    {
        string logFileName = $"{DateTime.Now:ddMMyyyy_HHmmss_ffff}.log";

        string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogPath, modelName, logFileName);

        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);

        await File.WriteAllTextAsync(logFilePath, $"{request}\n{response}", cancellationToken);
    }
}
