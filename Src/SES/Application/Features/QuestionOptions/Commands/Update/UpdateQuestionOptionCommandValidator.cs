using FluentValidation;

namespace Application.Features.QuestionOptions.Commands.Update;

public class UpdateQuestionOptionCommandValidator : AbstractValidator<UpdateQuestionOptionCommand>
{
    public UpdateQuestionOptionCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.QuizQuestionId).NotEmpty();
        RuleFor(c => c.IsCorrect).NotEmpty();
    }
}