using Application.Services.AIService;
using Application.Services.ImageService;
using Infrastructure.Adapters.AIService;
using Infrastructure.Adapters.ImageService;
using Infrastructure.Adapters.MediaService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ImageServiceBase, CloudinaryImageServiceAdapter>();
        services.AddKeyedScoped<IAIService, OpenAIServiceAdapter>("OpenAI");
        
        services.AddHttpClient<ZeroFileMediaServiceAdapter>()
            .ConfigureHttpClient((sp, client) =>
            {
                IConfiguration config = sp.GetRequiredService<IConfiguration>();
                client.BaseAddress = new Uri(config["ZeroFile:ServiceAddress"]!);
            });

        return services;
    }
}
