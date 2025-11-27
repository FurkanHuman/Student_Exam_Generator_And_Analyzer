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

    public IAIService GetService(string provider)
    {
        return provider switch
        {
            "OpenAI" => _serviceProvider.GetRequiredKeyedService<IAIService>("OpenAI"),
            "Anthropic" => _serviceProvider.GetRequiredKeyedService<IAIService>("Anthropic"),
            "Google" => _serviceProvider.GetRequiredKeyedService<IAIService>("Google"),
            "Azure" => _serviceProvider.GetRequiredKeyedService<IAIService>("Azure"),
            _ => throw new ArgumentException($"Unsupported AI provider: {provider}")
        };
    }
}
