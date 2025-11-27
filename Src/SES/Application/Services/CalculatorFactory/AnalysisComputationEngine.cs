using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

public class AnalysisComputationEngine(
    IAnalysisService analysisService,
    IAnalysisCalculatorFactory<AnalysisGeneralExamStatisticsCounters> generalStatsCalculator,
    IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> detailTableCalculator,
    IAnalysisCalculatorFactory<Dictionary<int, double>> questionAveragesCalculator,
    IAnalysisCalculatorFactory<SuccessDistribution> successDistributionCalculator,
    IAnalysisCalculatorFactory<AdvancedStatisticsData> advancedChartsCalculator,
    IAnalysisCalculatorFactory<List<List<StudentPerformanceDto>>> clusteringCalculator,
    IAnalysisCalculatorFactory<BenefitAnalysisResultDto> benefitAnalysisCalculator,
    IAnalysisCalculatorFactory<BenefitRadarResultDto> radarCalculator
        ) : AnalysisCalculatorFactory<int>(analysisService)
{
    public IAnalysisCalculatorFactory<AnalysisGeneralExamStatisticsCounters> GeneralStatsCalculator { get; set; } = generalStatsCalculator;
    public IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> DetailTableCalculator { get; set; } = detailTableCalculator;
    public IAnalysisCalculatorFactory<Dictionary<int, double>> QuestionAveragesCalculator { get; set; } = questionAveragesCalculator;
    public IAnalysisCalculatorFactory<SuccessDistribution> SuccessDistributionCalculator { get; set; } = successDistributionCalculator;
    public IAnalysisCalculatorFactory<AdvancedStatisticsData> AdvancedChartsCalculator { get; set; } = advancedChartsCalculator;
    public IAnalysisCalculatorFactory<List<List<StudentPerformanceDto>>> ClusteringCalculator { get; set; } = clusteringCalculator;
    public IAnalysisCalculatorFactory<BenefitAnalysisResultDto> BenefitAnalysisCalculator { get; set; } = benefitAnalysisCalculator;
    public IAnalysisCalculatorFactory<BenefitRadarResultDto> RadarCalculator { get; set; } = radarCalculator;

    public override async Task<int> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        return await base.CalculateAsync(analysisId, cancellationToken);
    }
}
