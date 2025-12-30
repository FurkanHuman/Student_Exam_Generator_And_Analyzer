using Application.Features.ExamConfigurations.Commands.Create;
using FluentValidation;

namespace Application.Features.Students.Commands.GhostCreate;

public class CreateGhostStudentCommandValidator : AbstractValidator<CreateGhostStudentCommand>
{
    public CreateGhostStudentCommandValidator()
    {
        RuleFor(x => x.SchoolId).GreaterThan(0);
        RuleFor(x => x.SemesterId).GreaterThan(0);
        RuleFor(x => x.RefTeacherId).GreaterThan(0);
        RuleFor(x => x.ClassAge).GreaterThan(0);
        RuleFor(x => x.ClassBranch).NotEmpty();
    }
}