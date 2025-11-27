using FluentValidation;

namespace Application.Features.Analyses.Commands.UpdateAIRequest;

public class UpdateAIRequestAnalysisCommandValidator : AbstractValidator<UpdateAIRequestAnalysisCommand>
{
    public UpdateAIRequestAnalysisCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().NotNull().NotEqual(0).LessThan(0);
        RuleFor(c => c.Model).NotEmpty().NotNull();
        RuleFor(c => c.Provider).NotEmpty().NotNull();
    }
}