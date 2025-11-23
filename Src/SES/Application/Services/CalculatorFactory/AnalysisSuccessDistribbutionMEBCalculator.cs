using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

public class AnalysisSuccessDistribbutionMEBCalculator : AnalysisCalculatorFactory<SuccessDistribution>, IAnalysisCalculatorFactory<SuccessDistribution>
{

    private readonly IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> _innerCalculator;
 
    public AnalysisSuccessDistribbutionMEBCalculator(IAnalysisService analysisService, IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> innerCalculator) : base(analysisService)
    {
        _innerCalculator = innerCalculator;
    }

    public override async Task<SuccessDistribution> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        await LoadPassingScoreAsync(analysisId, cancellationToken);

        IList<AnalysisDetailTableDto> details = await _innerCalculator.CalculateAsync(analysisId, cancellationToken);
        IList<int> nummbersOfSuccess = [.. details.SelectMany(adt => adt.Students).Select(ss => ss.StudentAnswerScores.Sum(s => s.GivenScore))];
        return new SuccessDistribution
        {
            Fail = nummbersOfSuccess.Count(s => s < PassingScore),
            Pass = nummbersOfSuccess.Count(s => s >= PassingScore && s < 60),
            Average = nummbersOfSuccess.Count(s => s >= 60 && s < 70),
            Good = nummbersOfSuccess.Count(s => s >= 70 && s < 85),
            VeryGood = nummbersOfSuccess.Count(s => s >= 85)
        };
    }
}

public class SuccessDistribution
{
    public int Fail { get; set; }
    public int Pass { get; set; }
    public int Average { get; set; }
    public int Good { get; set; }
    public int VeryGood { get; set; }
}