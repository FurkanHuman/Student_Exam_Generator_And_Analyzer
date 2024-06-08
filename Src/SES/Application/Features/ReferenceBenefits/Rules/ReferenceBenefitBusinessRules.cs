using Application.Features.ReferenceBenefits.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.ReferenceBenefits.Rules;

public class ReferenceBenefitBusinessRules : BaseBusinessRules
{
    private readonly IReferenceBenefitRepository _referenceBenefitRepository;
    private readonly ILocalizationService _localizationService;

    public ReferenceBenefitBusinessRules(IReferenceBenefitRepository referenceBenefitRepository, ILocalizationService localizationService)
    {
        _referenceBenefitRepository = referenceBenefitRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, ReferenceBenefitsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task ReferenceBenefitShouldExistWhenSelected(ReferenceBenefit? referenceBenefit)
    {
        if (referenceBenefit == null)
            await throwBusinessException(ReferenceBenefitsBusinessMessages.ReferenceBenefitNotExists);
    }

    public async Task ReferenceBenefitIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        ReferenceBenefit? referenceBenefit = await _referenceBenefitRepository.GetAsync(
            predicate: rb => rb.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await ReferenceBenefitShouldExistWhenSelected(referenceBenefit);
    }
}