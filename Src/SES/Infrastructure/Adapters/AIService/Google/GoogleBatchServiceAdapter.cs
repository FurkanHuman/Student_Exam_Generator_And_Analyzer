using Application.Services.AIService;
using Application.Services.AIService.Models;
using Google.GenAI;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.AIService.Google;

internal class GoogleBatchServiceAdapter : IAIBatchService
{
    private readonly Client _batchClient;
    private readonly BatchServiceConfiguration _batchServiceConfiguration;

    public GoogleBatchServiceAdapter(IConfiguration configuration)
    {
        _batchClient = new(apiKey: configuration.GetSection("GoogleApiKey").Get<string>());
        _batchServiceConfiguration = configuration.GetSection("BatchService").Get<BatchServiceConfiguration>() ?? new BatchServiceConfiguration();
    }

    public Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(List<AIAnalysisRequest> requests, string aiModel, CancellationToken cancellationToken)
    {
        // not implemented yet
        return Task.FromResult(new List<AIAnalysisResponse>());
    }
}
