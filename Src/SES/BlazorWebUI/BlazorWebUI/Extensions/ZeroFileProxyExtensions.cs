using Application.Services.ImageService;

namespace BlazorWebUI.Extensions;

public static class ZeroFileProxyExtensions
{
    public static IEndpointRouteBuilder MapZeroFileProxy(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/proxy/media/{id}", async (string id, IImageService imageService) =>
        {
            try
            {
                (byte[] FileBytes, Dictionary<string, object> Meta) = await imageService.DownloadFileAsync(id, CancellationToken.None);

                if (FileBytes == null || FileBytes.Length == 0)
                    return Results.NotFound();

                string mime = "";

                if (Meta != null && Meta.TryGetValue("Content-Type", out var ct))
                    mime = ct?.ToString() ?? "image/jpeg";

                return Results.File(FileBytes, mime);
            }
            catch
            {
                return Results.NotFound();
            }
        });

        return endpoints;
    }
}
