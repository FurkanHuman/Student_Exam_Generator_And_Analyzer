namespace BlazorWebUI;

public class BlazorConfiguration
{
    public string ApiDomain { get; set; }
    public string[] AllowedOrigins { get; set; }

    public BlazorConfiguration()
    {
        ApiDomain = string.Empty;
        AllowedOrigins = Array.Empty<string>();
    }

    public BlazorConfiguration(string apiDomain, string[] allowedOrigins)
    {
        ApiDomain = apiDomain;
        AllowedOrigins = allowedOrigins;
    }
}
