using Entity.Entities.Mains;

namespace App.PdfPageProduct.AnalysisPageFeature.V1;

internal static class AnalsysPageV1MicroCalc
{
    private static readonly string[] ColorForNotes = ["#00FF00", "#E0FF00", "#FFF000", "#FF8000", "#FF1010", "#000000"];

    internal static string ScoreColorForScore(QuestionScore questionScore) => ScoreColorForScore((double)questionScore.Score, questionScore.MaxScore);
    
    internal static IEnumerable<StudentQuizAnswer> SelectedStudents(AnalysisHeader analysisHeader, char[] crayzChar) => analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c)));

    internal static List<int> AverageScoresPieGraph(AnalysisHeader analysisHeader, char[] crayzChar) => ScoreSum(analysisHeader, crayzChar);

    internal static string RowColorChangeForInt(int sequenceNumber)
    {
        return (sequenceNumber % 2) switch
        {
            0 => "#4fc3f7",
            _ => "#b3e5fc",
        };
    }

    internal static string ScoreColorForScore(double questionScore, int refScore)
    {
        double ratio = (double)questionScore / refScore;
        return ratio switch
        {
            >= 0.8 => ColorForNotes[0],
            >= 0.6 and < 0.8 => ColorForNotes[1],
            >= 0.4 and < 0.6 => ColorForNotes[2],
            >= 0.2 and < 0.4 => ColorForNotes[3],
            _ => ColorForNotes[4]
        };
    }

    internal static string ScoreColorForTotalScore(int totalScore)
    {
        return totalScore switch
        {
            >= 85 => ColorForNotes[0],
            >= 70 and < 85 => ColorForNotes[1],
            >= 60 and < 70 => ColorForNotes[2],
            >= 50 and < 60 => ColorForNotes[3],
            _ => ColorForNotes[4]
        };
    }

    internal static List<int> ScoreSum(AnalysisHeader analysisHeader, char[] crayzChar)
    {
        List<int> scores = new();
        foreach (StudentQuizAnswer studentQuizAnswer in SelectedStudents(analysisHeader, crayzChar))

            switch (studentQuizAnswer.TotalScore)
            {
                case >= 85:
                    scores.Add(5);
                    break;
                case >= 70 and < 85:
                    scores.Add(4);
                    break;
                case >= 60 and < 70:
                    scores.Add(3);
                    break;
                case >= 50 and < 60:
                    scores.Add(2);
                    break;
                default:
                    scores.Add(1);
                    break;
            }

        return scores;
    }

    internal static List<double> AverageScoresBarGraph(AnalysisHeader analysisHeader, char[] crayzChar)
    {
        List<double> averageScores = [];
        int questionCount = analysisHeader.RefScorePerQuestions.Length;

        for (int j = 0; j < questionCount; j++)
            averageScores.Add(CalculateClassAveragesPerQuestion(analysisHeader, j, crayzChar));

        return averageScores;
    }

    internal static double CalculateClassAveragesPerQuestion(AnalysisHeader analysisHeader, int index, char[] crayzChar)
    {
        int totalScore = 0;

        foreach (StudentQuizAnswer studentQuizAnswer in SelectedStudents(analysisHeader, crayzChar))
            totalScore += studentQuizAnswer.ScorePerQuestions[index].Score;

        return (double)totalScore / SelectedStudents(analysisHeader, crayzChar).Count();
    }
}
