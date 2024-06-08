using Application.Features.Analyses.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Analyses.Rules;

public class AnalysisBusinessRules : BaseBusinessRules
{
    private readonly IAnalysisRepository _analysisRepository;
    private readonly ILocalizationService _localizationService;

    public AnalysisBusinessRules(IAnalysisRepository analysisRepository, ILocalizationService localizationService)
    {
        _analysisRepository = analysisRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, AnalysesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task AnalysisShouldExistWhenSelected(Analysis? analysis)
    {
        if (analysis == null)
            await throwBusinessException(AnalysesBusinessMessages.AnalysisNotExists);
    }

    public async Task AnalysisIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Analysis? analysis = await _analysisRepository.GetAsync(
            predicate: a => a.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await AnalysisShouldExistWhenSelected(analysis);
    }
}