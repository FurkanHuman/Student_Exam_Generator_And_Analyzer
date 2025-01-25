using FluentValidation;

namespace Application.Features.ReferenceBenefits.Commands.Create;

public class CreateReferenceBenefitCommandValidator : AbstractValidator<CreateReferenceBenefitCommand>
{
    public CreateReferenceBenefitCommandValidator()
    {
        RuleFor(c => c.ReferenceBenefitDto.RBName).NotNull().NotEmpty();
        RuleFor(c => c.ReferenceBenefitDto.LessonId).NotEmpty();
        RuleFor(c => c.ReferenceBenefitDto.SchoolId).NotEmpty();
        RuleFor(c => c.ReferenceBenefitDto.SemesterId).NotEmpty();
    }
}