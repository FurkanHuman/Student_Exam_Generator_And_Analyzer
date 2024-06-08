using FluentValidation;

namespace Application.Features.Semesters.Commands.Delete;

public class DeleteSemesterCommandValidator : AbstractValidator<DeleteSemesterCommand>
{
    public DeleteSemesterCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}