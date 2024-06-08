using FluentValidation;

namespace Application.Features.Benefits.Commands.Delete;

public class DeleteBenefitCommandValidator : AbstractValidator<DeleteBenefitCommand>
{
    public DeleteBenefitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}