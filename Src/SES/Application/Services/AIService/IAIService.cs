using Application.Services.AIService.Models;

namespace Application.Services.AIService;
public interface IAIService
{
    Task<ICollection<QuestionAIInComingModel>?> GenerateQuestionsFromAIAsync(QuestionAIOutgoingModel outgoingModel, string aiModel, CancellationToken cancellationToken);
}
