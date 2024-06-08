using FluentValidation;

namespace Application.Features.QuizQuestions.Commands.Delete;

public class DeleteQuizQuestionCommandValidator : AbstractValidator<DeleteQuizQuestionCommand>
{
    public DeleteQuizQuestionCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}