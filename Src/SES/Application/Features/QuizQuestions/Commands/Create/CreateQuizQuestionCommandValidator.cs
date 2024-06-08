using FluentValidation;

namespace Application.Features.QuizQuestions.Commands.Create;

public class CreateQuizQuestionCommandValidator : AbstractValidator<CreateQuizQuestionCommand>
{
    public CreateQuizQuestionCommandValidator()
    {
        RuleFor(c => c.BenefitId).NotEmpty();
        RuleFor(c => c.ExamId).NotEmpty();
        RuleFor(c => c.Score).NotEmpty();
        RuleFor(c => c.Question).NotEmpty();
        RuleFor(c => c.QuestionBody).NotEmpty();
        RuleFor(c => c.QuestionImage).NotEmpty();
        RuleFor(c => c.QuestionType).NotEmpty();
    }
}