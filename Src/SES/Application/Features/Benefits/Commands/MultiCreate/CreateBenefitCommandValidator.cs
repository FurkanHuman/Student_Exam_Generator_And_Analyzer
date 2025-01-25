using FluentValidation;

namespace Application.Features.Benefits.Commands.MultiCreate;

public class MultiCreateBenefitCommandValidator : AbstractValidator<MultiCreateBenefitCommand>
{
    public MultiCreateBenefitCommandValidator()
    {
        RuleFor(c => c.MultiBenefits.Select(m=>m.BenefitCode)).NotEmpty();
        RuleFor(c => c.MultiBenefits.Select(m => m.Description)).NotEmpty();
    }
}