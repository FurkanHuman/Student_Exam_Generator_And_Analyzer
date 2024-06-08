using FluentValidation;

namespace Application.Features.StudentClasses.Commands.Delete;

public class DeleteStudentClassCommandValidator : AbstractValidator<DeleteStudentClassCommand>
{
    public DeleteStudentClassCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}