using Application.Features.QuestionScores.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.QuestionScores.Rules;

public class QuestionScoreBusinessRules : BaseBusinessRules
{
    private readonly IQuestionScoreRepository _questionScoreRepository;
    private readonly ILocalizationService _localizationService;

    public QuestionScoreBusinessRules(IQuestionScoreRepository questionScoreRepository, ILocalizationService localizationService)
    {
        _questionScoreRepository = questionScoreRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, QuestionScoresBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task QuestionScoreShouldExistWhenSelected(QuestionScore? questionScore)
    {
        if (questionScore == null)
            await throwBusinessException(QuestionScoresBusinessMessages.QuestionScoreNotExists);
    }

    public async Task QuestionScoreIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        QuestionScore? questionScore = await _questionScoreRepository.GetAsync(
            predicate: qs => qs.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await QuestionScoreShouldExistWhenSelected(questionScore);
    }
}