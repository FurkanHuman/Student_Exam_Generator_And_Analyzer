using FluentValidation;

namespace Application.Features.Personels.Commands.Update;

public class UpdatePersonelCommandValidator : AbstractValidator<UpdatePersonelCommand>
{
    public UpdatePersonelCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.SurName).NotEmpty();
        RuleFor(c => c.BirthDate).NotEmpty();
    }
}