using Application.Features.FeedBacks.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.FeedBacks.Rules;

public class FeedBackBusinessRules : BaseBusinessRules
{
    private readonly IFeedBackRepository _feedBackRepository;
    private readonly ILocalizationService _localizationService;

    public FeedBackBusinessRules(IFeedBackRepository feedBackRepository, ILocalizationService localizationService)
    {
        _feedBackRepository = feedBackRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, FeedBacksBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task FeedBackShouldExistWhenSelected(FeedBack? feedBack)
    {
        if (feedBack == null)
            await throwBusinessException(FeedBacksBusinessMessages.FeedBackNotExists);
    }

    public async Task FeedBackIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        FeedBack? feedBack = await _feedBackRepository.GetAsync(
            predicate: fb => fb.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await FeedBackShouldExistWhenSelected(feedBack);
    }
}