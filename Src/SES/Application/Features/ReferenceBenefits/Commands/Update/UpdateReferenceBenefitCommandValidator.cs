using FluentValidation;

namespace Application.Features.ReferenceBenefits.Commands.Update;

public class UpdateReferenceBenefitCommandValidator : AbstractValidator<UpdateReferenceBenefitCommand>
{
    public UpdateReferenceBenefitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.ReferenceBenefitName).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.ExamId).NotEmpty();
        RuleFor(c => c.LearningAreaId).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
    }
}