using Application.Services.AIService.Models;

namespace Application.Services.AIService;
public interface IAIService
{
    Task<AIAnalysisResponse> GenerateAnalysisFromAIAsync(AIAnalysisRequest analysisRequest, string provider, string aiModel, CancellationToken cancellationToken);
    Task<ICollection<QuestionAIInComingModel>?> GenerateQuestionsFromAIAsync(QuestionAIOutgoingModel outgoingModel, string aiModel, CancellationToken cancellationToken);
}
