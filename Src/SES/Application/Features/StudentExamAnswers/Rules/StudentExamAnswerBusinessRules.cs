using Application.Features.StudentAnswers.Constants;
using Application.Features.StudentExamAnswers.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.StudentExamAnswers.Rules;

public class StudentExamAnswerBusinessRules : BaseBusinessRules
{
    private readonly IStudentExamAnswerRepository _studentExamAnswerRepository;
    private readonly ILocalizationService _localizationService;

    public StudentExamAnswerBusinessRules(IStudentExamAnswerRepository studentExamAnswerRepository, ILocalizationService localizationService)
    {
        _studentExamAnswerRepository = studentExamAnswerRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, StudentExamAnswersBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task StudentExamAnswerShouldExistWhenSelected(StudentExamAnswer? studentExamAnswer)
    {
        if (studentExamAnswer == null)
            await throwBusinessException(StudentExamAnswersBusinessMessages.StudentExamAnswerNotExists);
    }

    public async Task StudentExamAnswerIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        StudentExamAnswer? studentExamAnswer = await _studentExamAnswerRepository.GetAsync(
            predicate: sea => sea.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await StudentExamAnswerShouldExistWhenSelected(studentExamAnswer);
    }
    internal async Task CheckIfReviewerTeacherExistsAsync(Teacher? teacher)
    {
        if (teacher == null)
            await throwBusinessException(StudentAnswersBusinessMessages.ReviewerTeacherNotExists);

    }
}