using FluentValidation;

namespace Application.Features.Students.Commands.Update;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.SurName).NotEmpty();
        RuleFor(c => c.ClassAge).NotEmpty();
        RuleFor(c => c.ClassBranch).NotEmpty();
        RuleFor(c => c.SchoolNumber).NotEmpty();
        RuleFor(c => c.Gender).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.TeacherId).NotEmpty();
        RuleFor(c => c.ExamId).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.School).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
    }
}