using Application.Services.ImageService;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Infrastructure.Adapters.MediaService;

public class ZeroFileMediaServiceAdapter : IImageServices
{
    private readonly HttpClient _httpClient;

    public ZeroFileMediaServiceAdapter(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["ZeroFile:ServiceAddress"]!);
    }

    public async Task<Dictionary<string, object>> UploadFileAsync(FileStream fileStream, CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new();

        await fileStream.CopyToAsync(memoryStream, cancellationToken);

        return await UploadFileAsync(memoryStream.ToArray(), Path.GetFileName(fileStream.Name), cancellationToken);
    }

    public async Task<Dictionary<string, object>> UploadFileAsync(byte[] fileBytes, string fileName, CancellationToken cancellationToken)
    {
        using MultipartFormDataContent form = [];
        ByteArrayContent fileContent = new(fileBytes);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        form.Add(fileContent, "file", fileName);

        using HttpResponseMessage response = await _httpClient.PostAsync($"/upload", form, cancellationToken);

        return await ReadJsonResponseAsync(response, cancellationToken);
    }

    public async Task<(byte[] FileBytes, Dictionary<string, object> Meta)> DownloadFileAsync(string id, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"/media/{id}", cancellationToken);

        byte[] bytesFile = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        Dictionary<string, object> meta = ReadFileHeaders(response);

        return (bytesFile, meta);
    }

    public async Task<Dictionary<string, object>> DeleteFileAsync(string id, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.DeleteAsync($"/media/{id}", cancellationToken);

        return await ReadJsonResponseAsync(response, cancellationToken);
    }

    public async Task<Dictionary<string, object>> GetFileMetaAsync(string id, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"/meta/{id}", cancellationToken);

        return await ReadJsonResponseAsync(response, cancellationToken);
    }

    public async Task<Dictionary<string, object>> GetHealthAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"/health", cancellationToken);

        return await ReadJsonResponseAsync(response, cancellationToken);
    }
    private static Dictionary<string, object> ReadFileHeaders(HttpResponseMessage response)
    {
        // 404 or empty content is returned as empty meta
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.Content.Headers.ContentLength == 0 || response.Headers == null)
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        Dictionary<string, object> meta = new(StringComparer.OrdinalIgnoreCase);
        foreach (KeyValuePair<string, IEnumerable<string>> header in from KeyValuePair<string, IEnumerable<string>> header in response.Headers
                                                                     where header.Key.StartsWith("X-", StringComparison.OrdinalIgnoreCase)
                                                                     select header)

        {
            meta[header.Key[2..]] = header.Value;
        }

        return meta;
    }

    // shared method to read JSON responses
    private static async Task<Dictionary<string, object>> ReadJsonResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        string json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(json))
            return [];

        try
        {
            Dictionary<string, object> data = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? [];

            data["url"] = response.RequestMessage?.RequestUri?.ToString() ?? string.Empty;

            return data.Count > 1 ? data : [];
        }
        catch (JsonException)
        {
            return [];
        }

    }

}