using FluentValidation;

namespace Application.Features.StudentClasses.Commands.Create;

public class CreateStudentClassCommandValidator : AbstractValidator<CreateStudentClassCommand>
{
    public CreateStudentClassCommandValidator()
    {
        RuleFor(c => c.ClassAge).NotEmpty();
        RuleFor(c => c.ClassBranch).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.RefTeacherId).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
        RuleFor(c => c.RefTeacher).NotEmpty();
    }
}