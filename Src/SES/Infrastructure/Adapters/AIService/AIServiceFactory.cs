using Application.Services.AIService;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Adapters.AIService;

public class AIServiceFactory : IAIServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public AIServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IAIBatchService GetBatchService(string provider)
    {
        ValidateProvider(provider);
        return _serviceProvider.GetRequiredKeyedService<IAIBatchService>(provider);
    }

    public IAIService GetService(string provider)
    {
        ValidateProvider(provider);
        return _serviceProvider.GetRequiredKeyedService<IAIService>(provider);
    }

    private static void ValidateProvider(string provider)
    {
        Dictionary<string, string> supportedProviders = AIModelsLoader.GetAIProviders();
        string availableProviders = string.Join(", ", supportedProviders.Keys);
        if (!supportedProviders.ContainsKey(provider))
            throw new ArgumentException($"Unsupported AI provider: '{provider}'. Available providers: {availableProviders}");

    }
}