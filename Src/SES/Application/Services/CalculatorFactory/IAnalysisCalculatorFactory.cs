
namespace Application.Services.CalculatorFactory;

public interface IAnalysisCalculatorFactory<TResult>
{
    Task<TResult> CalculateAsync(int analysisId, CancellationToken cancellationToken = default);
}