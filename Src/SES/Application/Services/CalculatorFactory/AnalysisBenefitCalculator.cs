using Application.Services.Analyses;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CalculatorFactory;

public class AnalysisBenefitCalculator : AnalysisCalculatorFactory<BenefitAnalysisResultDto>
{
    private readonly IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> _detailCalculator;
    private const double MaxBenefitScore = 4.0; // 4 based on the scoring system

    public AnalysisBenefitCalculator(
        IAnalysisService analysisService,
        IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> detailCalculator)
        : base(analysisService)
    {
        _detailCalculator = detailCalculator;
    }

    public override async Task<BenefitAnalysisResultDto> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {

        IList<AnalysisDetailTableDto> detailTables = await _detailCalculator.CalculateAsync(analysisId, cancellationToken);

        Analysis? analysis = await _analysisService.GetAsync(
            predicate: a => a.Id == analysisId,
            include: a => a
                .Include(a => a.Exams)
                    .ThenInclude(e => e.QuizQuestions)
                        .ThenInclude(qq => qq.Benefits),
            cancellationToken: cancellationToken
        );

        if (analysis == null)
            throw new InvalidOperationException($"Analysis with Id {analysisId} not found.");

        BenefitAnalysisResultDto result = new();

        List<StudentTableDto> allStudents = detailTables.SelectMany(t => t.Students).ToList();

        foreach (StudentTableDto student in allStudents)
        {
            StudentBenefitAnalysisDto studentAnalysis = CalculateStudentBenefits(student, analysis);
            result.StudentAnalyses.Add(studentAnalysis);
        }

        result.ClassAverageBenefits = CalculateClassAverageBenefits(result.StudentAnalyses);

        result.BenefitStatusDistribution = CalculateBenefitStatusDistribution(result.ClassAverageBenefits);

        return result;
    }

    private StudentBenefitAnalysisDto CalculateStudentBenefits(StudentTableDto student, Analysis analysis)
    {
        StudentBenefitAnalysisDto studentAnalysis = new StudentBenefitAnalysisDto
        {
            StudentId = student.Id,
            StudentName = $"{student.Name} {student.Surname}"
        };

        List<Benefit> allBenefits = [.. analysis.Exams
            .SelectMany(e => e.QuizQuestions)
            .SelectMany(qq => qq.Benefits ?? Enumerable.Empty<Benefit>())
            .DistinctBy(b => b.Id)];

        foreach (Benefit benefit in allBenefits)
        {
            BenefitScoreDto benefitScore = CalculateBenefitScoreForStudent(benefit, student, analysis);
            studentAnalysis.BenefitScores.Add(benefitScore);
        }

        if (studentAnalysis.BenefitScores.Count != 0)
            studentAnalysis.AverageBenefitScore = studentAnalysis.BenefitScores.Average(b => b.Score);

        else
            studentAnalysis.AverageBenefitScore = 0;

        return studentAnalysis;
    }

    private BenefitScoreDto CalculateBenefitScoreForStudent(Benefit benefit, StudentTableDto student, Analysis analysis)
    {
        BenefitScoreDto benefitScore = new()
        {
            BenefitCode = benefit.BenefitCode ?? "",
            Description = benefit.Description ?? ""
        };

        List<QuizQuestion> questionsWithBenefit = [.. analysis.Exams
            .SelectMany(e => e.QuizQuestions)
            .Where(qq => qq.Benefits != null && qq.Benefits.Any(b => b.Id == benefit.Id))];

        benefitScore.TotalQuestions = questionsWithBenefit.Count;

        double totalBenefitScore = 0;

        foreach (QuizQuestion question in questionsWithBenefit)
        {
            int k = question.Benefits?.Count ?? 1;

            StudentAnswerScore? studentAnswer = student.StudentAnswerScores
                .FirstOrDefault(sa => sa.QuestionId == question.Id);

            double questionScore = studentAnswer?.GivenScore ?? 0;

            double contribution = (1.0 / k) * questionScore;

            benefitScore.QuestionContributions[question.Id] = contribution;

            totalBenefitScore += contribution;
        }

        benefitScore.Score = Math.Min(totalBenefitScore, MaxBenefitScore);
        benefitScore.Percentage = (benefitScore.Score / MaxBenefitScore) * 100;

        return benefitScore;
    }

    private List<BenefitScoreDto> CalculateClassAverageBenefits(List<StudentBenefitAnalysisDto> studentAnalyses)
    {
        if (studentAnalyses.Count == 0)
            return [];

        List<string> allBenefitCodes = [.. studentAnalyses
            .SelectMany(sa => sa.BenefitScores)
            .Select(b => b.BenefitCode)
            .Distinct()];

        List<BenefitScoreDto> classAverages = [];

        foreach (string benefitCode in allBenefitCodes)
        {
            List<BenefitScoreDto> benefitScores = [.. studentAnalyses
                .SelectMany(sa => sa.BenefitScores)
                .Where(b => b.BenefitCode == benefitCode)];

            if (benefitScores.Count != 0)
            {
                classAverages.Add(new BenefitScoreDto
                {
                    BenefitCode = benefitCode,
                    Description = benefitScores.First().Description,
                    Score = benefitScores.Average(b => b.Score),
                    Percentage = benefitScores.Average(b => b.Percentage),
                    TotalQuestions = benefitScores.First().TotalQuestions
                });
            }
        }

        return classAverages.OrderBy(b => b.BenefitCode).ToList();
    }

    private static Dictionary<string, int> CalculateBenefitStatusDistribution(List<BenefitScoreDto> benefits)
    {
        Dictionary<string, int> distribution = new()
        {
            ["Çok İyi"] = 0,
            ["İyi"] = 0,
            ["Yeterli"] = 0,
            ["Gelişmeli"] = 0,
            ["Yetersiz"] = 0
        };

        foreach (BenefitScoreDto benefit in benefits)
        {
            string status = GetBenefitStatus(benefit.Score);
            distribution[status]++;
        }

        return distribution;
    }

    private static string GetBenefitStatus(double score)
    {
        return score switch
        {
            >= 3.5 => "Çok İyi",
            >= 3.0 => "İyi",
            >= 2.5 => "Yeterli",
            >= 2.0 => "Gelişmeli",
            _ => "Yetersiz"
        };
    }
}
public class BenefitScoreDto
{
    public string BenefitCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Score { get; set; }
    public double Percentage { get; set; }
    public int TotalQuestions { get; set; }
    public Dictionary<int, double> QuestionContributions { get; set; } = new();
}

public class StudentBenefitAnalysisDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public List<BenefitScoreDto> BenefitScores { get; set; } = new();
    public double AverageBenefitScore { get; set; }
}

public class BenefitAnalysisResultDto
{
    public List<StudentBenefitAnalysisDto> StudentAnalyses { get; set; } = new();
    public List<BenefitScoreDto> ClassAverageBenefits { get; set; } = new();
    public Dictionary<string, int> BenefitStatusDistribution { get; set; } = new();
}