using Application.Services.AIService.Models;

namespace Application.Services.AIService;

public interface IAIBatchService
{
    Task<List<AIAnalysisResponse>> GenerateBatchAnalysisAsync(List<AIAnalysisRequest> aIAnalysisRequests, string model, CancellationToken cancellationToken);
}