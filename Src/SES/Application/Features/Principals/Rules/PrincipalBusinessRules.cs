using Application.Features.Principals.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Principals.Rules;

public class PrincipalBusinessRules : BaseBusinessRules
{
    private readonly IPrincipalRepository _principalRepository;
    private readonly ILocalizationService _localizationService;

    public PrincipalBusinessRules(IPrincipalRepository principalRepository, ILocalizationService localizationService)
    {
        _principalRepository = principalRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, PrincipalsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task PrincipalShouldExistWhenSelected(Principal? principal)
    {
        if (principal == null)
            await throwBusinessException(PrincipalsBusinessMessages.PrincipalNotExists);
    }

    public async Task PrincipalIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Principal? principal = await _principalRepository.GetAsync(
            predicate: p => p.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await PrincipalShouldExistWhenSelected(principal);
    }
}