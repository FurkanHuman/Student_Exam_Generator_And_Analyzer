using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

internal class AnalysisLessonPassingScoreCalculator : AnalysisCalculatorFactory<int>
{
    public AnalysisLessonPassingScoreCalculator(IAnalysisService analysisService) : base(analysisService)
    {
    }

    public override async Task<int> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        return await base.CalculateAsync(analysisId, cancellationToken);
    }
}
