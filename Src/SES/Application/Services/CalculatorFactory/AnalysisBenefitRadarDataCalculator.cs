using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

public class BenefitRadarDataCalculator : AnalysisCalculatorFactory<BenefitRadarResultDto>
{
    private static double MaxBenefitScore => 4.0;
    private readonly IAnalysisCalculatorFactory<BenefitAnalysisResultDto> _benefitCalculator;

    public BenefitRadarDataCalculator(IAnalysisService analysisService, IAnalysisCalculatorFactory<BenefitAnalysisResultDto> benefitCalculator) : base(analysisService)
    {
        _benefitCalculator = benefitCalculator;
    }

    public override async Task<BenefitRadarResultDto> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        await LoadPassingScoreAsync(analysisId, cancellationToken);

        BenefitAnalysisResultDto benefitAnalysis = await _benefitCalculator.CalculateAsync(analysisId, cancellationToken);

        BenefitRadarResultDto result = new()
        {
            ClassAverageRadar = BuildClassAverageRadar(benefitAnalysis),
            StudentRadars = BuildStudentRadars(benefitAnalysis)
        };

        return result;
    }

    private static RadarChartData BuildClassAverageRadar(BenefitAnalysisResultDto benefitAnalysis)
    {
        RadarChartData radarData = new()
        {
            Label = "Sınıf Ortalaması",
            IsClassAverage = true
        };

        foreach (BenefitScoreDto? benefit in benefitAnalysis.ClassAverageBenefits.OrderBy(b => b.BenefitCode))
        {
            radarData.Categories.Add(benefit.BenefitCode);
            radarData.Values.Add(benefit.Score);
            radarData.MaxValue = MaxBenefitScore;
        }

        return radarData;
    }

    private static List<StudentRadarData> BuildStudentRadars(BenefitAnalysisResultDto benefitAnalysis)
    {
        List<StudentRadarData> studentRadars = [];
        IOrderedEnumerable<BenefitScoreDto> classBenefits = benefitAnalysis.ClassAverageBenefits.OrderBy(b => b.BenefitCode);

        foreach (StudentBenefitAnalysisDto studentAnalysis in benefitAnalysis.StudentAnalyses)
        {
            StudentRadarData radar = new()
            {
                StudentId = studentAnalysis.StudentId,
                StudentName = studentAnalysis.StudentName,
                AverageScore = studentAnalysis.AverageBenefitScore
            };

            foreach (BenefitScoreDto? classBenefit in classBenefits)
            {
                BenefitScoreDto? studentBenefit = studentAnalysis.BenefitScores
                    .FirstOrDefault(b => b.BenefitCode == classBenefit.BenefitCode);

                radar.Categories.Add(classBenefit.BenefitCode);
                radar.Values.Add(studentBenefit?.Score ?? 0);
            }

            radar.MaxValue = MaxBenefitScore;
            studentRadars.Add(radar);
        }

        return studentRadars;
    }
}

public class BenefitRadarResultDto
{
    public RadarChartData ClassAverageRadar { get; set; } = new();
    public List<StudentRadarData> StudentRadars { get; set; } = [];
}

public class RadarChartData
{
    public string Label { get; set; } = string.Empty;
    public bool IsClassAverage { get; set; }
    public List<string> Categories { get; set; } = [];
    public List<double> Values { get; set; } = [];
    public double MaxValue { get; set; } = 4.0;
}

public class StudentRadarData : RadarChartData
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public double AverageScore { get; set; }
}
