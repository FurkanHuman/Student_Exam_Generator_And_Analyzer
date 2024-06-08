using FluentValidation;

namespace Application.Features.Exams.Commands.Update;

public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.LessonName).NotEmpty();
        RuleFor(c => c.ExamCode).NotEmpty();
        RuleFor(c => c.FooterNote).NotEmpty();
        RuleFor(c => c.SemesterId).NotEmpty();
        RuleFor(c => c.AnalysisId).NotEmpty();
        RuleFor(c => c.TeacherId).NotEmpty();
        RuleFor(c => c.StudentId).NotEmpty();
        RuleFor(c => c.SchoolId).NotEmpty();
        RuleFor(c => c.ReferenceBenefitId).NotEmpty();
        RuleFor(c => c.Semester).NotEmpty();
        RuleFor(c => c.Teacher).NotEmpty();
        RuleFor(c => c.Student).NotEmpty();
        RuleFor(c => c.School).NotEmpty();
        RuleFor(c => c.ReferenceBenefit).NotEmpty();
    }
}