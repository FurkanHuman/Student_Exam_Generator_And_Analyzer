using Application.Features.LearningAreas.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.LearningAreas.Rules;

public class LearningAreaBusinessRules : BaseBusinessRules
{
    private readonly ILearningAreaRepository _learningAreaRepository;
    private readonly ILocalizationService _localizationService;

    public LearningAreaBusinessRules(ILearningAreaRepository learningAreaRepository, ILocalizationService localizationService)
    {
        _learningAreaRepository = learningAreaRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, LearningAreasBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task LearningAreaShouldExistWhenSelected(LearningArea? learningArea)
    {
        if (learningArea == null)
            await throwBusinessException(LearningAreasBusinessMessages.LearningAreaNotExists);
    }

    public async Task LearningAreaIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        LearningArea? learningArea = await _learningAreaRepository.GetAsync(
            predicate: la => la.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await LearningAreaShouldExistWhenSelected(learningArea);
    }
}