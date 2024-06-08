using FluentValidation;

namespace Application.Features.LearningAreas.Commands.Delete;

public class DeleteLearningAreaCommandValidator : AbstractValidator<DeleteLearningAreaCommand>
{
    public DeleteLearningAreaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}