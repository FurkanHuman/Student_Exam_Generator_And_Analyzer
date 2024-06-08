using FluentValidation;

namespace Application.Features.Teachers.Commands.Delete;

public class DeleteTeacherCommandValidator : AbstractValidator<DeleteTeacherCommand>
{
    public DeleteTeacherCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}