using FluentValidation;

namespace Application.Features.Benefits.Commands.Create;

public class CreateBenefitCommandValidator : AbstractValidator<CreateBenefitCommand>
{
    public CreateBenefitCommandValidator()
    {
        RuleFor(c => c.SubLearningAreaId).NotEmpty();
        RuleFor(c => c.ReferenceBenefitNumber).NotEmpty();
        RuleFor(c => c.ReferenceBenefitComments).NotEmpty();
    }
}