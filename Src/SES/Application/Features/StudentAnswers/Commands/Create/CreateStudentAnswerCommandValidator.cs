using FluentValidation;

namespace Application.Features.StudentAnswers.Commands.Create;

public class CreateStudentAnswerCommandValidator : AbstractValidator<CreateStudentAnswerCommand>
{
    public CreateStudentAnswerCommandValidator()
    {
        RuleFor(c => c.StudentId).NotEmpty();
        RuleFor(c => c.QuizQuestionId).NotEmpty();
        RuleFor(c => c.Score).NotEmpty();
        RuleFor(c => c.IsCorrect).NotEmpty();
    }
}