using Application.Features.Benefits.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Benefits.Rules;

public class BenefitBusinessRules : BaseBusinessRules
{
    private readonly IBenefitRepository _benefitRepository;
    private readonly ILocalizationService _localizationService;

    public BenefitBusinessRules(IBenefitRepository benefitRepository, ILocalizationService localizationService)
    {
        _benefitRepository = benefitRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, BenefitsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task BenefitShouldExistWhenSelected(Benefit? benefit)
    {
        if (benefit == null)
            await throwBusinessException(BenefitsBusinessMessages.BenefitNotExists);
    }

    public async Task BenefitIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Benefit? benefit = await _benefitRepository.GetAsync(
            predicate: b => b.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await BenefitShouldExistWhenSelected(benefit);
    }
}