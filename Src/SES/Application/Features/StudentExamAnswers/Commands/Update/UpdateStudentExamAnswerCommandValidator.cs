using FluentValidation;

namespace Application.Features.StudentExamAnswers.Commands.Update;

public class UpdateStudentExamAnswerCommandValidator : AbstractValidator<UpdateStudentExamAnswerCommand>
{
    public UpdateStudentExamAnswerCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.ReviewerTeacherId).NotEmpty();
        RuleFor(c => c.StudentId).NotEmpty();
        RuleFor(c => c.ExamId).NotEmpty();
        RuleFor(c => c.EvaluationOrigin).NotEmpty();
        RuleFor(c => c.ExamEvaluationStatus).NotEmpty();
    }
}