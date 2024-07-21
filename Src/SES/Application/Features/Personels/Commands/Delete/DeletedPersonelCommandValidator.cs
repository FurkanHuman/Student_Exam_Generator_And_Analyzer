using FluentValidation;

namespace Application.Features.Personels.Commands.Delete;

public class DeletePersonelCommandValidator : AbstractValidator<DeletePersonelCommand>
{
    public DeletePersonelCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}