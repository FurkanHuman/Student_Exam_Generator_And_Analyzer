using FluentValidation;

namespace Application.Features.ExamConfigurations.Commands.Update;

public class UpdateExamConfigurationCommandValidator : AbstractValidator<UpdateExamConfigurationCommand>
{
    public UpdateExamConfigurationCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.ConfigurationHash).NotEmpty();
    }
}