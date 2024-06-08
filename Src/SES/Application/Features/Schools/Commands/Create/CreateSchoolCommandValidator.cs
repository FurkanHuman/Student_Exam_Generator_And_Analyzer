using FluentValidation;

namespace Application.Features.Schools.Commands.Create;

public class CreateSchoolCommandValidator : AbstractValidator<CreateSchoolCommand>
{
    public CreateSchoolCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Principal).NotEmpty();
    }
}