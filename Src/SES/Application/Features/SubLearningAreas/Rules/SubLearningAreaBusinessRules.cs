using Application.Features.SubLearningAreas.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.SubLearningAreas.Rules;

public class SubLearningAreaBusinessRules : BaseBusinessRules
{
    private readonly ISubLearningAreaRepository _subLearningAreaRepository;
    private readonly ILocalizationService _localizationService;

    public SubLearningAreaBusinessRules(ISubLearningAreaRepository subLearningAreaRepository, ILocalizationService localizationService)
    {
        _subLearningAreaRepository = subLearningAreaRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, SubLearningAreasBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task SubLearningAreaShouldExistWhenSelected(SubLearningArea? subLearningArea)
    {
        if (subLearningArea == null)
            await throwBusinessException(SubLearningAreasBusinessMessages.SubLearningAreaNotExists);
    }

    public async Task SubLearningAreaIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        SubLearningArea? subLearningArea = await _subLearningAreaRepository.GetAsync(
            predicate: sla => sla.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await SubLearningAreaShouldExistWhenSelected(subLearningArea);
    }
}