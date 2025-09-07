using FluentValidation;

namespace Application.Features.StudentExamAnswers.Commands.Create;

public class CreateStudentExamAnswerCommandValidator : AbstractValidator<CreateStudentExamAnswerCommand>
{
    public CreateStudentExamAnswerCommandValidator()
    {
        RuleFor(c => c.ReviewerPersonelId).NotEmpty().NotNull();
        RuleFor(c => c.StudentId).NotEmpty().NotNull();
        RuleFor(c => c.ExamId).NotEmpty().NotNull();
        RuleFor(c => c.EvaluationOrigin).NotEmpty().NotNull();
        RuleFor(c => c.ExamEvaluationStatus).NotEmpty().NotNull();
        RuleFor(c => c.StudentQuestionAnswers).NotEmpty().NotNull();
    }
}