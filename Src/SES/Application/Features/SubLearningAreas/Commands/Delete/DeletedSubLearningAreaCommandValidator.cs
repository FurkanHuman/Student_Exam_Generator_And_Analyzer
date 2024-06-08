using FluentValidation;

namespace Application.Features.SubLearningAreas.Commands.Delete;

public class DeleteSubLearningAreaCommandValidator : AbstractValidator<DeleteSubLearningAreaCommand>
{
    public DeleteSubLearningAreaCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}