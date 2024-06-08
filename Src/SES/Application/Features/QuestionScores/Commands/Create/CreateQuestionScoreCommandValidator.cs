using FluentValidation;

namespace Application.Features.QuestionScores.Commands.Create;

public class CreateQuestionScoreCommandValidator : AbstractValidator<CreateQuestionScoreCommand>
{
    public CreateQuestionScoreCommandValidator()
    {
        RuleFor(c => c.Score).NotEmpty();
        RuleFor(c => c.MaxScore).NotEmpty();
    }
}