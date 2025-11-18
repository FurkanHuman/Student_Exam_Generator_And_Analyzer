using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

public class AnalysisQuestionAveragesCalculator : AnalysisCalculatorFactory<Dictionary<int, double>>
{
    private readonly IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> _innerCalculator;

    public AnalysisQuestionAveragesCalculator(IAnalysisService analysisService, IAnalysisCalculatorFactory<IList<AnalysisDetailTableDto>> innerCalculator) : base(analysisService) => _innerCalculator = innerCalculator;

    public override async Task<Dictionary<int, double>> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        IList<AnalysisDetailTableDto> tables = await _innerCalculator.CalculateAsync(analysisId, cancellationToken);
        Dictionary<int, List<double>> allScores = [];

        foreach (AnalysisDetailTableDto table in tables)
        {
            Dictionary<int, double> questionScores = CalculateQuestionAveragesForExam(table);

            foreach (KeyValuePair<int, double> kvp in questionScores)
            {
                if (!allScores.ContainsKey(kvp.Key))
                    allScores[kvp.Key] = [];

                allScores[kvp.Key].Add(kvp.Value);
            }
        }

        return allScores.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Average());
    }

    private static Dictionary<int, double> CalculateQuestionAveragesForExam(AnalysisDetailTableDto table)
    {
        Dictionary<int, double> averages = [];

        if (table.Students == null || !table.Students.Any())
            return averages;

        int maxQuestionCount = table.QuestionCount;

        for (int questionOrder = 1; questionOrder <= maxQuestionCount; questionOrder++)
        {
            int questionIndex = questionOrder - 1;

            List<int> scoresForQuestion = [.. table.Students
                .Where(s => s.StudentAnswerScores != null && s.StudentAnswerScores.Count > questionIndex)
                .Select(s => s.StudentAnswerScores[questionIndex].GivenScore)];

            if (scoresForQuestion.Count != 0)
            {
                double average = scoresForQuestion.Average();
                averages[questionOrder] = average;
            }
        }
        return averages;
    }
}