using FluentValidation;

namespace Application.Features.Exams.Commands.CreateMultipleExam;

public class CreateMultipleExamCommandValidator : AbstractValidator<CreateMultipleExamCommand>
{
    public CreateMultipleExamCommandValidator()
    {
        RuleFor(x => x.ExamInfo).NotEmpty().NotNull().WithMessage("ExamInfo cannot be null.");
        RuleFor(x => x.LessonId).NotEmpty().WithMessage("LessonId cannot be empty.");
        RuleFor(x => x.SemesterId).NotEmpty().WithMessage("SemesterId cannot be empty.");
        RuleFor(x => x.ClassIds).NotEmpty().WithMessage("ClassIds cannot be empty.");
    }
}