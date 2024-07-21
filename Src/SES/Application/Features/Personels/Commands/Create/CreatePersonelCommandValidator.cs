using FluentValidation;

namespace Application.Features.Personels.Commands.Create;

public class CreatePersonelCommandValidator : AbstractValidator<CreatePersonelCommand>
{
    public CreatePersonelCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.SurName).NotEmpty();
        RuleFor(c => c.BirthDate).NotEmpty();
    }
}