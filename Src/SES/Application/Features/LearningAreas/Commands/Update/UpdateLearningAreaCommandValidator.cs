using FluentValidation;

namespace Application.Features.LearningAreas.Commands.Update;

public class UpdateLearningAreaCommandValidator : AbstractValidator<UpdateLearningAreaCommand>
{
    public UpdateLearningAreaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.SubLearningAreaId).NotEmpty();
    }
}