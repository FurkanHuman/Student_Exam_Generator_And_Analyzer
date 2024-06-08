using Application.Features.QuestionOptions.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.QuestionOptions.Rules;

public class QuestionOptionBusinessRules : BaseBusinessRules
{
    private readonly IQuestionOptionRepository _questionOptionRepository;
    private readonly ILocalizationService _localizationService;

    public QuestionOptionBusinessRules(IQuestionOptionRepository questionOptionRepository, ILocalizationService localizationService)
    {
        _questionOptionRepository = questionOptionRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, QuestionOptionsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task QuestionOptionShouldExistWhenSelected(QuestionOption? questionOption)
    {
        if (questionOption == null)
            await throwBusinessException(QuestionOptionsBusinessMessages.QuestionOptionNotExists);
    }

    public async Task QuestionOptionIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        QuestionOption? questionOption = await _questionOptionRepository.GetAsync(
            predicate: qo => qo.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await QuestionOptionShouldExistWhenSelected(questionOption);
    }
}