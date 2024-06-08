using FluentValidation;

namespace Application.Features.LearningAreas.Commands.Create;

public class CreateLearningAreaCommandValidator : AbstractValidator<CreateLearningAreaCommand>
{
    public CreateLearningAreaCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.SubLearningAreaId).NotEmpty();
    }
}