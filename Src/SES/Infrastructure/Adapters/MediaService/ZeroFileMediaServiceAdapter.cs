using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Infrastructure.Adapters.MediaService;

public class ZeroFileMediaServiceAdapter
{
    private readonly HttpClient _httpClient;

    public ZeroFileMediaServiceAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // shared method to read JSON responses
    private static async Task<Dictionary<string, string>> ReadJsonResponse(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        string json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(json))
            return [];

        try
        {
            Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return data != null && data.Count > 0 ? data : [];
        }

        catch (JsonException)
        {
            return [];
        }
    }

    public async Task<Dictionary<string, string>> UploadFile(byte[] fileBytes, string fileName, CancellationToken cancellationToken)
    {
        using MultipartFormDataContent form = [];
        
        ByteArrayContent fileContent = new(fileBytes);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        form.Add(fileContent, "file", fileName);

        using HttpResponseMessage response = await _httpClient.PostAsync($"/upload", form, cancellationToken);
        
        response.EnsureSuccessStatusCode();

        return await ReadJsonResponse(response, cancellationToken);
    }

    public async Task<(byte[], Dictionary<string, string>)> DownloadFile(string id, CancellationToken cancellationToken)
    {
        Dictionary<string, string> meta = await GetFileMeta(id, cancellationToken);

        using HttpResponseMessage response = await _httpClient.GetAsync($"/media/{id}", cancellationToken);
        
        response.EnsureSuccessStatusCode();

        byte[] bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        if (bytes.Length > 0 || meta.Count > 0)
            return (bytes, meta);

        return ([], []);
    }

    public async Task<Dictionary<string, string>> DeleteFile(string id, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.DeleteAsync($"/media/{id}", cancellationToken);
        
        response.EnsureSuccessStatusCode();

        return await ReadJsonResponse(response, cancellationToken);
    }

    public async Task<Dictionary<string, string>> GetFileMeta(string id, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"/meta/{id}", cancellationToken);
        
        response.EnsureSuccessStatusCode();

        return await ReadJsonResponse(response, cancellationToken);
    }

    public async Task<Dictionary<string, string>> GetHealth(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"/health", cancellationToken);
        
        response.EnsureSuccessStatusCode();

        return await ReadJsonResponse(response, cancellationToken);
    }
}

