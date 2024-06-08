using FluentValidation;

namespace Application.Features.Analyses.Commands.Update;

public class UpdateAnalysisCommandValidator : AbstractValidator<UpdateAnalysisCommand>
{
    public UpdateAnalysisCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.ClassAge).NotEmpty();
        RuleFor(c => c.AltClass).NotEmpty();
        RuleFor(c => c.ExamSemesterYear).NotEmpty();
        RuleFor(c => c.LessonName).NotEmpty();
        RuleFor(c => c.LessonSession).NotEmpty();
        RuleFor(c => c.ExamCode).NotEmpty();
        RuleFor(c => c.FooterNote).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.BenefitId).NotEmpty();
        RuleFor(c => c.QuestionId).NotEmpty();
        RuleFor(c => c.TeacherId).NotEmpty();
        RuleFor(c => c.PrincipalId).NotEmpty();
        RuleFor(c => c.StudentQuizAnswerId).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
        RuleFor(c => c.Teacher).NotEmpty();
        RuleFor(c => c.Principal).NotEmpty();
        RuleFor(c => c.School).NotEmpty();
    }
}