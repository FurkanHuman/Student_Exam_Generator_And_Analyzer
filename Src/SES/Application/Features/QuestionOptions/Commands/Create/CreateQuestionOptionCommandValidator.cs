using FluentValidation;

namespace Application.Features.QuestionOptions.Commands.Create;

public class CreateQuestionOptionCommandValidator : AbstractValidator<CreateQuestionOptionCommand>
{
    public CreateQuestionOptionCommandValidator()
    {
        RuleFor(c => c.QuizQuestionId).NotEmpty();
        RuleFor(c => c.IsCorrect).NotEmpty();
    }
}