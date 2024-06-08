using FluentValidation;

namespace Application.Features.StudentAnswers.Commands.Delete;

public class DeleteStudentAnswerCommandValidator : AbstractValidator<DeleteStudentAnswerCommand>
{
    public DeleteStudentAnswerCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}