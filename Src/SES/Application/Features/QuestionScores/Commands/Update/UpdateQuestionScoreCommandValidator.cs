using FluentValidation;

namespace Application.Features.QuestionScores.Commands.Update;

public class UpdateQuestionScoreCommandValidator : AbstractValidator<UpdateQuestionScoreCommand>
{
    public UpdateQuestionScoreCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Score).NotEmpty();
        RuleFor(c => c.MaxScore).NotEmpty();
    }
}