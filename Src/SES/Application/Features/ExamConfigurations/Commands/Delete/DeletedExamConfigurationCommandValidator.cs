using FluentValidation;

namespace Application.Features.ExamConfigurations.Commands.Delete;

public class DeleteExamConfigurationCommandValidator : AbstractValidator<DeleteExamConfigurationCommand>
{
    public DeleteExamConfigurationCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}