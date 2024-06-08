using FluentValidation;

namespace Application.Features.Analyses.Commands.Delete;

public class DeleteAnalysisCommandValidator : AbstractValidator<DeleteAnalysisCommand>
{
    public DeleteAnalysisCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}