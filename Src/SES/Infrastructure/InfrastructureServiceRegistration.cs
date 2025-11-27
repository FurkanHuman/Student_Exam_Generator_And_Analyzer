using Application.Services.AIService;
using Application.Services.ImageService;
using Infrastructure.Adapters.AIService;
using Infrastructure.Adapters.ImageService;
using Infrastructure.Adapters.MediaService;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ImageServiceBase, CloudinaryImageServiceAdapter>();
        services.AddScoped<IImageService, ZeroFileMediaServiceAdapter>();
        services.AddScoped<IAIServiceFactory, AIServiceFactory>();
        services.AddKeyedScoped<IAIService, OpenAIServiceAdapter>("OpenAI");

        services.AddHttpClient<ZeroFileMediaServiceAdapter>();
        return services;
    }
}
