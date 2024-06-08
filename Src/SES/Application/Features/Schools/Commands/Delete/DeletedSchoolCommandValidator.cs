using FluentValidation;

namespace Application.Features.Schools.Commands.Delete;

public class DeleteSchoolCommandValidator : AbstractValidator<DeleteSchoolCommand>
{
    public DeleteSchoolCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}