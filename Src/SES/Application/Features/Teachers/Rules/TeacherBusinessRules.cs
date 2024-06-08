using Application.Features.Teachers.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Teachers.Rules;

public class TeacherBusinessRules : BaseBusinessRules
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly ILocalizationService _localizationService;

    public TeacherBusinessRules(ITeacherRepository teacherRepository, ILocalizationService localizationService)
    {
        _teacherRepository = teacherRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, TeachersBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task TeacherShouldExistWhenSelected(Teacher? teacher)
    {
        if (teacher == null)
            await throwBusinessException(TeachersBusinessMessages.TeacherNotExists);
    }

    public async Task TeacherIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Teacher? teacher = await _teacherRepository.GetAsync(
            predicate: t => t.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await TeacherShouldExistWhenSelected(teacher);
    }
}