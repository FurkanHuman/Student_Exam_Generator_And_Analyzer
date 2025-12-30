using FluentValidation;

namespace Application.Features.ExamConfigurations.Commands.Create;

public class CreateExamConfigurationCommandValidator : AbstractValidator<CreateExamConfigurationCommand>
{
    public CreateExamConfigurationCommandValidator()
    {
        RuleFor(c => c.ConfigurationHash).NotEmpty();
    }
}