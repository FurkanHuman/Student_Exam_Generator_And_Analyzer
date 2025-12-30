using Application.Services.AIService.Models;

namespace Application.Services.AIService;

public interface IAIService
{
    Task<AIAnalysisResponse> GenerateAnalysisFromAIAsync(AIAnalysisRequest analysisRequest, string aiModel, CancellationToken cancellationToken);
    Task<List<AIQuestionGenerationResponse>> GenerateQuestionsFromAIAsync(AIQuestionGenerationRequest generationRequest, string aiModel, CancellationToken cancellationToken);
    Task<List<AIQuestionGenerationResponse>> ExtractQuestionsFromDocumentAsync(AIQuestionExtractionRequest extractionRequest, string aiModel, CancellationToken cancellationToken);

}
