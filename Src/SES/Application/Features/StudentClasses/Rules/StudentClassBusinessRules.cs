using Application.Features.StudentClasses.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.StudentClasses.Rules;

public class StudentClassBusinessRules : BaseBusinessRules
{
    private readonly IStudentClassRepository _studentClassRepository;
    private readonly ILocalizationService _localizationService;

    public StudentClassBusinessRules(IStudentClassRepository studentClassRepository, ILocalizationService localizationService)
    {
        _studentClassRepository = studentClassRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, StudentClassesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task StudentClassShouldExistWhenSelected(StudentClass? studentClass)
    {
        if (studentClass == null)
            await throwBusinessException(StudentClassesBusinessMessages.StudentClassNotExists);
    }

    public async Task StudentClassIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        StudentClass? studentClass = await _studentClassRepository.GetAsync(
            predicate: sc => sc.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await StudentClassShouldExistWhenSelected(studentClass);
    }
}