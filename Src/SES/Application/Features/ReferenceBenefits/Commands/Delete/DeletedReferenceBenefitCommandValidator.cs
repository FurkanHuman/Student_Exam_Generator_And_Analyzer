using FluentValidation;

namespace Application.Features.ReferenceBenefits.Commands.Delete;

public class DeleteReferenceBenefitCommandValidator : AbstractValidator<DeleteReferenceBenefitCommand>
{
    public DeleteReferenceBenefitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}