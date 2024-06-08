using FluentValidation;

namespace Application.Features.Benefits.Commands.Update;

public class UpdateBenefitCommandValidator : AbstractValidator<UpdateBenefitCommand>
{
    public UpdateBenefitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.SubLearningAreaId).NotEmpty();
        RuleFor(c => c.ReferenceBenefitNumber).NotEmpty();
        RuleFor(c => c.ReferenceBenefitComments).NotEmpty();
    }
}