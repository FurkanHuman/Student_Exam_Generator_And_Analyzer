using FluentValidation;

namespace Application.Features.ReferenceBenefits.Commands.Create;

public class CreateReferenceBenefitCommandValidator : AbstractValidator<CreateReferenceBenefitCommand>
{
    public CreateReferenceBenefitCommandValidator()
    {
        RuleFor(c => c.ReferenceBenefitName).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.ExamId).NotEmpty();
        RuleFor(c => c.LearningAreaId).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
    }
}