using Application.Services.AIService;
using Application.Services.ImageService;
using Infrastructure.Adapters.AIService;
using Infrastructure.Adapters.ImageService;
using Infrastructure.Adapters.MediaService;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ImageServiceBase, CloudinaryImageServiceAdapter>();
        services.AddScoped<IImageService, ZeroFileMediaServiceAdapter>();
        services.AddScoped<IAIServiceFactory, AIServiceFactory>();

        // AI Services - Convention-based registration
        services.AddAIServices();

        return services;
    }

    private static IServiceCollection AddAIServices(this IServiceCollection services)
    {
        Dictionary<string, string> providers = AIModelsLoader.GetAIProviders();
        Assembly assembly = typeof(InfrastructureServiceRegistration).Assembly;

        foreach (string providerKey in providers.Keys)
        {
            string serviceTypeName = $"Infrastructure.Adapters.AIService.{providerKey}.{providerKey}ServiceAdapter";
            string batchServiceTypeName = $"Infrastructure.Adapters.AIService.{providerKey}.{providerKey}BatchServiceAdapter";

            Type? serviceType = assembly.GetType(serviceTypeName);
            Type? batchServiceType = assembly.GetType(batchServiceTypeName);

            if (serviceType == null && batchServiceType == null)
                throw new InvalidOperationException(
                        $"Provider '{providerKey}' found in CSV but adapter classes not found.\nExpected: {serviceTypeName} and {batchServiceTypeName}");

            if (serviceType != null)
                services.AddKeyedScoped(typeof(IAIService), providerKey, serviceType);

            if (batchServiceType != null)
                services.AddKeyedScoped(typeof(IAIBatchService), providerKey, batchServiceType);
        }
        return services;
    }
}