using FluentValidation;

namespace Application.Features.StudentClasses.Commands.Update;

public class UpdateStudentClassCommandValidator : AbstractValidator<UpdateStudentClassCommand>
{
    public UpdateStudentClassCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.ClassAge).NotEmpty();
        RuleFor(c => c.ClassBranch).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.RefTeacherId).NotEmpty();
        RuleFor(c => c.School).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
        RuleFor(c => c.RefTeacher).NotEmpty();
    }
}