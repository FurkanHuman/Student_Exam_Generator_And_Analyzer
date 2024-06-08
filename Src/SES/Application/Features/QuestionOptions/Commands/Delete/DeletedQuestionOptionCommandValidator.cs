using FluentValidation;

namespace Application.Features.QuestionOptions.Commands.Delete;

public class DeleteQuestionOptionCommandValidator : AbstractValidator<DeleteQuestionOptionCommand>
{
    public DeleteQuestionOptionCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}