using Application.Features.Schools.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Schools.Rules;

public class SchoolBusinessRules : BaseBusinessRules
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly ILocalizationService _localizationService;

    public SchoolBusinessRules(ISchoolRepository schoolRepository, ILocalizationService localizationService)
    {
        _schoolRepository = schoolRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, SchoolsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task SchoolShouldExistWhenSelected(School? school)
    {
        if (school == null)
            await throwBusinessException(SchoolsBusinessMessages.SchoolNotExists);
    }

    public async Task SchoolIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        School? school = await _schoolRepository.GetAsync(
            predicate: s => s.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await SchoolShouldExistWhenSelected(school);
    }
}