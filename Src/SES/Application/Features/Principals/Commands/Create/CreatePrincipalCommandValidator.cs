using FluentValidation;

namespace Application.Features.Principals.Commands.Create;

public class CreatePrincipalCommandValidator : AbstractValidator<CreatePrincipalCommand>
{
    public CreatePrincipalCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.SurName).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.School).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
    }
}