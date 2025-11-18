using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

public abstract class AnalysisCalculatorFactory<TResult>(IAnalysisService analysisService) : IAnalysisCalculatorFactory<TResult>
{
    protected readonly IAnalysisService _analysisService = analysisService;

    public virtual Task<TResult> CalculateAsync(int analysisId, CancellationToken cancellationToken = default) => Task.FromResult(default(TResult)!);
}