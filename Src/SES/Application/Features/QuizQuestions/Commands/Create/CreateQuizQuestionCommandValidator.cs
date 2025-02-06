using FluentValidation;

namespace Application.Features.QuizQuestions.Commands.Create;

public class CreateQuizQuestionCommandValidator : AbstractValidator<CreateQuizQuestionCommand>
{
    public CreateQuizQuestionCommandValidator()
    {
        RuleFor(c => c.BenefitIds).NotEmpty();
        RuleFor(c => c.Score).NotEmpty();
        RuleFor(c => c.MaxScore).NotEmpty();
        RuleFor(c => c.Question).NotEmpty();
        RuleFor(c => c.QuestionType).NotEmpty();
    }
}