using Application.Features.Personels.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Personels.Rules;

public class PersonelBusinessRules : BaseBusinessRules
{
    private readonly IPersonelRepository _personelRepository;
    private readonly ILocalizationService _localizationService;

    public PersonelBusinessRules(IPersonelRepository personelRepository, ILocalizationService localizationService)
    {
        _personelRepository = personelRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, PersonelsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task PersonelShouldExistWhenSelected(Personel? personel)
    {
        if (personel == null)
            await throwBusinessException(PersonelsBusinessMessages.PersonelNotExists);
    }

    public async Task PersonelIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Personel? personel = await _personelRepository.GetAsync(
            predicate: p => p.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await PersonelShouldExistWhenSelected(personel);
    }
}