using Application.Features.ExamConfigurations.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.ExamConfigurations.Rules;

public class ExamConfigurationBusinessRules : BaseBusinessRules
{
    private readonly IExamConfigurationRepository _examConfigurationRepository;
    private readonly ILocalizationService _localizationService;

    public ExamConfigurationBusinessRules(IExamConfigurationRepository examConfigurationRepository, ILocalizationService localizationService)
    {
        _examConfigurationRepository = examConfigurationRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, ExamConfigurationsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task ExamConfigurationShouldExistWhenSelected(ExamConfiguration? examConfiguration)
    {
        if (examConfiguration == null)
            await throwBusinessException(ExamConfigurationsBusinessMessages.ExamConfigurationNotExists);
    }

    public async Task ExamConfigurationIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        ExamConfiguration? examConfiguration = await _examConfigurationRepository.GetAsync(
            predicate: ec => ec.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await ExamConfigurationShouldExistWhenSelected(examConfiguration);
    }
}