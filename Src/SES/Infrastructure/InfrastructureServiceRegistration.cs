using Application.Services.AIService;
using Application.Services.ImageService;
using Infrastructure.Adapters.AIService;
using Infrastructure.Adapters.ImageService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ImageServiceBase, CloudinaryImageServiceAdapter>();
        services.AddKeyedScoped<IAIService, OpenAIServiceAdapter>("OpenAI");
        
        return services;
    }
}
