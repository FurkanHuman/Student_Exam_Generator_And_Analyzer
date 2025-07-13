using FluentValidation;

namespace Application.Features.StudentAnswers.Commands.CreateMultiple;

public class CreateMultipleStudentAnswerCommandValidator : AbstractValidator<CreateMultipleStudentAnswerCommand>
{
    public CreateMultipleStudentAnswerCommandValidator()
    {
        RuleFor(x => x.StudentAnswers)
            .NotEmpty().NotNull()
            .WithMessage("Student answers cannot be empty.");

        RuleForEach(x => x.StudentAnswers).ChildRules(studentAnswer =>
        {
            studentAnswer.RuleFor(x => x.StudentId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Student ID must be greater than 0.");
            studentAnswer.RuleFor(x => x.QuizQuestionId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Quiz question ID must be greater than 0.");
            studentAnswer.RuleFor(x => x.GivenScore)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Given score must be greater than or equal to 0.");
            studentAnswer.RuleFor(x => x.EvaluationOrigin)
                .NotNull()
                .NotEmpty()
                .InclusiveBetween((byte)0, (byte)6)
                .WithMessage("Evaluation origin must be either 0 or 6.");
            studentAnswer.RuleFor(x => x.EvaluationStatus)
                .NotNull()
                .NotEmpty()
                .InclusiveBetween((byte)0, (byte)9)
                .WithMessage("Evaluation status must be between 0 and 9.");
        });
    }
}