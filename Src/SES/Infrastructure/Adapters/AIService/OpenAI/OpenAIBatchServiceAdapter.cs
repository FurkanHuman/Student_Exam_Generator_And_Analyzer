using Application.Services.AIService;
using Application.Services.AIService.Models;
using Microsoft.Extensions.Configuration;
using OpenAI.Batch;
using System.Text.Json;

namespace Infrastructure.Adapters.AIService.OpenAI;

#pragma warning disable OPENAI001
internal class OpenAIBatchServiceAdapter : IAIBatchService
{
    private readonly BatchClient _batchClient;
    private readonly BatchServiceConfiguration _batchServiceConfiguration;

    public OpenAIBatchServiceAdapter(IConfiguration configuration)
    {
        _batchClient = new(apiKey: configuration.GetSection("OpenAiApiKey").Get<string>());
        _batchServiceConfiguration = configuration.GetSection("BatchService").Get<BatchServiceConfiguration>() ?? new BatchServiceConfiguration();
    }

    public Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(List<AIAnalysisRequest> aIAnalysisRequests, string model, CancellationToken cancellationToken)
    {
        // not implemented yet
        return Task.FromResult(new List<AIAnalysisResponse>());
    }

    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };
}
#pragma warning restore OPENAI001