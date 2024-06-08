using Application.Features.StudentAnswers.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.StudentAnswers.Rules;

public class StudentAnswerBusinessRules : BaseBusinessRules
{
    private readonly IStudentAnswerRepository _studentAnswerRepository;
    private readonly ILocalizationService _localizationService;

    public StudentAnswerBusinessRules(IStudentAnswerRepository studentAnswerRepository, ILocalizationService localizationService)
    {
        _studentAnswerRepository = studentAnswerRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, StudentAnswersBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task StudentAnswerShouldExistWhenSelected(StudentAnswer? studentAnswer)
    {
        if (studentAnswer == null)
            await throwBusinessException(StudentAnswersBusinessMessages.StudentAnswerNotExists);
    }

    public async Task StudentAnswerIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        StudentAnswer? studentAnswer = await _studentAnswerRepository.GetAsync(
            predicate: sa => sa.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await StudentAnswerShouldExistWhenSelected(studentAnswer);
    }
}