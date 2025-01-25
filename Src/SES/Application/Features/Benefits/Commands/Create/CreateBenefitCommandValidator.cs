using FluentValidation;

namespace Application.Features.Benefits.Commands.Create;

public class CreateBenefitCommandValidator : AbstractValidator<CreateBenefitCommand>
{
    public CreateBenefitCommandValidator()
    {
        RuleFor(c => c.BenefitCode).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        
    }
}