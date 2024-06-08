using FluentValidation;

namespace Application.Features.QuestionScores.Commands.Delete;

public class DeleteQuestionScoreCommandValidator : AbstractValidator<DeleteQuestionScoreCommand>
{
    public DeleteQuestionScoreCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}