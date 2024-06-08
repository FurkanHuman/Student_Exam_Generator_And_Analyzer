using FluentValidation;

namespace Application.Features.SubLearningAreas.Commands.Update;

public class UpdateSubLearningAreaCommandValidator : AbstractValidator<UpdateSubLearningAreaCommand>
{
    public UpdateSubLearningAreaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.BenefitId).NotEmpty();
    }
}