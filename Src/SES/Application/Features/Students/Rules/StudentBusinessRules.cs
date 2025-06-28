using Application.Features.Students.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Students.Rules;

public class StudentBusinessRules : BaseBusinessRules
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILocalizationService _localizationService;

    public StudentBusinessRules(IStudentRepository studentRepository, ILocalizationService localizationService)
    {
        _studentRepository = studentRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, StudentsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task StudentShouldExistWhenSelected(Student? student)
    {
        if (student == null)
            await throwBusinessException(StudentsBusinessMessages.StudentNotExists);
    }

    public async Task StudentIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        Student? student = await _studentRepository.GetAsync(
            predicate: s => s.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await StudentShouldExistWhenSelected(student);
    }

    internal async Task IdsShouldNotBeEmpty(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())

            await throwBusinessException(StudentsBusinessMessages.IdsShouldNotBeEmpty);
    }

    internal async Task StudentsShouldExistWhenSelected(IPaginate<Student> students)
    {
        if (students == null || students.Items.Count == 0)
            await throwBusinessException(StudentsBusinessMessages.StudentNotExists);

    }
}