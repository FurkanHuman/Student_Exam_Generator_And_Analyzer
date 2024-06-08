using FluentValidation;

namespace Application.Features.Principals.Commands.Delete;

public class DeletePrincipalCommandValidator : AbstractValidator<DeletePrincipalCommand>
{
    public DeletePrincipalCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}