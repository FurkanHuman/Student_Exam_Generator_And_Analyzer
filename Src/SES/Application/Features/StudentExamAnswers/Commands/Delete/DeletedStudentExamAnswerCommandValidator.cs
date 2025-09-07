using FluentValidation;

namespace Application.Features.StudentExamAnswers.Commands.Delete;

public class DeleteStudentExamAnswerCommandValidator : AbstractValidator<DeleteStudentExamAnswerCommand>
{
    public DeleteStudentExamAnswerCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}