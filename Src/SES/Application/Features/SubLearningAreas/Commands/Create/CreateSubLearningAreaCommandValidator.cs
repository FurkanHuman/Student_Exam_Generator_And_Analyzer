using FluentValidation;

namespace Application.Features.SubLearningAreas.Commands.Create;

public class CreateSubLearningAreaCommandValidator : AbstractValidator<CreateSubLearningAreaCommand>
{
    public CreateSubLearningAreaCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.BenefitId).NotEmpty();
    }
}