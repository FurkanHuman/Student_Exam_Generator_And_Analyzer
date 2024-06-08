using FluentValidation;

namespace Application.Features.Principals.Commands.Update;

public class UpdatePrincipalCommandValidator : AbstractValidator<UpdatePrincipalCommand>
{
    public UpdatePrincipalCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.SurName).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.School).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
    }
}