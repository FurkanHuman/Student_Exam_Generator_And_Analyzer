using FluentValidation;

namespace Application.Features.Principals.Commands.Create;

public class CreatePrincipalCommandValidator : AbstractValidator<CreatePrincipalCommand>
{
    public CreatePrincipalCommandValidator()
    {
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.PersonelId).NotEmpty();

    }
}