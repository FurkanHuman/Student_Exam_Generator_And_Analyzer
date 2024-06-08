using FluentValidation;

namespace Application.Features.StudentAnswers.Commands.Update;

public class UpdateStudentAnswerCommandValidator : AbstractValidator<UpdateStudentAnswerCommand>
{
    public UpdateStudentAnswerCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.StudentId).NotEmpty();
        RuleFor(c => c.QuizQuestionId).NotEmpty();
        RuleFor(c => c.Score).NotEmpty();
        RuleFor(c => c.IsCorrect).NotEmpty();
    }
}