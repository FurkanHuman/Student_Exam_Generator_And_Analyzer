using Application.Features.Semesters.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Semesters.Rules;

public class SemesterBusinessRules : BaseBusinessRules
{
    private readonly ISemesterRepository _semesterRepository;
    private readonly ILocalizationService _localizationService;

    public SemesterBusinessRules(ISemesterRepository semesterRepository, ILocalizationService localizationService)
    {
        _semesterRepository = semesterRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, SemestersBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task SemesterShouldExistWhenSelected(Semester? semester)
    {
        if (semester == null)
            await throwBusinessException(SemestersBusinessMessages.SemesterNotExists);
    }

    public async Task SemesterIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Semester? semester = await _semesterRepository.GetAsync(
            predicate: s => s.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await SemesterShouldExistWhenSelected(semester);
    }
}