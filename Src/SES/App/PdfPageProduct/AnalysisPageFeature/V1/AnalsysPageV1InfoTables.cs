using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.AnalysisPageFeature.V1;

internal static class AnalsysPageV1InfoTables
{
    internal static void StudentPreTop(AnalysisHeader analysisHeader, ColumnDescriptor generalClassInfoForStudent)
    {
        generalClassInfoForStudent.Item().Background(Colors.Green.Accent2).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(10)).Row(generalClassInfoForStudentC =>
        {
            generalClassInfoForStudentC.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Toplam öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Count}");

            generalClassInfoForStudentC.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınava girmeyen öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s =>
            s.SpecialStatuses.Contains('g')).Count()}");

            generalClassInfoForStudentC.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınava Tekrardan Girebilecek öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s =>
            s.SpecialStatuses.Contains('r') | s.SpecialStatuses.Contains('m')).Count()}");

            generalClassInfoForStudentC.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınavda başarılı öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s =>
            s.TotalScore >= 50 & (!s.SpecialStatuses.Contains('k') | !s.SpecialStatuses.Contains('g'))).Count()}");

            generalClassInfoForStudentC.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınavda başarısız öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s =>
            s.TotalScore < 50 | s.SpecialStatuses.Contains('k') | s.SpecialStatuses.Contains('g')).Count()}");
        });
    }

    internal static void TopInfo(ColumnDescriptor generalClassInfoForStudent, int[] refScores)
    {
        generalClassInfoForStudent.Item().Background(Colors.Cyan.Lighten3).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(10)).Border(0.5f, Unit.Millimetre).Row(rst =>
        {
            rst.RelativeItem(3.5f).Row(studentRow =>
            {
                studentRow.RelativeItem(1).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text("#S");
                studentRow.RelativeItem(1).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text("#No");
                studentRow.RelativeItem(3).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text("Adı");
                studentRow.RelativeItem(3).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text("Soyadı");
            });

            rst.RelativeItem(9).Row(studentScore =>
            {
                for (int i = 0; i < refScores.Length; i++)
                    studentScore.RelativeItem(1).Border(0.25f, Unit.Millimetre).AlignCenter().Text($"{i + 1}\n({refScores[i]})");

                studentScore.ConstantItem(1.25f, Unit.Centimetre).Border(0.25f, Unit.Millimetre).AlignCenter().Text($"Toplam");
                studentScore.ConstantItem(2, Unit.Centimetre).Border(0.25f, Unit.Millimetre).AlignCenter().Text($"Durum");
            });
        });
    }

    internal static void DrawStudensAndNotes(AnalysisHeader analysisHeader, ColumnDescriptor generalClassInfoForStudent)
    {
        int sequence = 1;
        foreach (StudentQuizAnswer studentQuizAnswer in analysisHeader.StudentQuizAnswers.OrderBy(o => o.StudentNumber))
        {
            StudensAndNotes(generalClassInfoForStudent, studentQuizAnswer, sequence, studentQuizAnswer.SpecialStatuses);
            sequence++;
        }
    }

    internal static void ClassScoreSummary(ColumnDescriptor generalClassInfoForStudent, AnalysisHeader analysisHeader, char[] crayzChar)
    {
        generalClassInfoForStudent.Item().Background(Colors.Pink.Lighten4).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(rst =>
        {
            rst.RelativeItem(3.5F).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text("Soru Başına Sınıf Ortalaması: ");
            rst.RelativeItem(9).Row(css =>
            {// soru başına sınıf ortalamsı
                QuestionPerDividedStudents(analysisHeader, css, crayzChar);
                int refStudentSum = analysisHeader.RefScorePerQuestions.Select(s => s).Sum();
                int studentCount = analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))).Count();
                double studentSum = Math.Round((double)analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))).Select(s => s.TotalScore).Sum() / studentCount, 2);

                css.ConstantItem(3.25f, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForScore(studentSum, refStudentSum)).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"Sınıf Ortalaması: {studentSum}");

            });
        });
        generalClassInfoForStudent.Item().Background(Colors.Pink.Lighten4).PaddingHorizontal(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(rst =>
        {
            rst.RelativeItem(3.5F).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text("Sınıf Başarı Tablosu: ");
            rst.RelativeItem(9).Row(css =>
            {
                List<int> scores = AnalsysPageV1MicroCalc.ScoreSum(analysisHeader, crayzChar);

                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(1) \"Geçmez\" : {scores.Where(p => p < 2).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(2) \"Geçer\" : {scores.Where(p => p >= 2 & p < 3).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(3) \"Orta\" : {scores.Where(p => p >= 3 & p < 4).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(4) \"İyi\" : {scores.Where(p => p >= 4 & p < 5).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(5) \"Pek İyi\" : {scores.Where(p => p >= 5).Count()}");

                css.ConstantItem(3.25f, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForScore(scores.Sum() / scores.Count, 5)).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"Sınıf Ortalaması: {Math.Round((double)scores.Sum() / scores.Count, 2)}");
            });
        });
    }


    private static void QuestionPerDividedStudents(AnalysisHeader analysisHeader, RowDescriptor css, char[] crayzChar)
    {
        for (int i = 0; i < analysisHeader.RefScorePerQuestions.Length; i++)
        {
            double stps = AnalsysPageV1MicroCalc.CalculateClassAveragesPerQuestion(analysisHeader, i, crayzChar);
            css.RelativeItem(1).Background(AnalsysPageV1MicroCalc.ScoreColorForScore(stps, analysisHeader.RefScorePerQuestions[i])).Border(0.25f, Unit.Millimetre).AlignMiddle().AlignCenter().Text($"{Math.Round(stps, 1)}");
        }
    }

    private static void StudensAndNotes(ColumnDescriptor generalClassInfoForStudent, StudentQuizAnswer studentQuizAnswer, int sequenceNumber, char[] crayzChar)
    {
        generalClassInfoForStudent.Item().Background(AnalsysPageV1MicroCalc.RowColorChangeForInt(sequenceNumber)).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.1f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(rst =>
        {
            rst.RelativeItem(3.5f).Row(studentRow =>

            { // öğrenci bilgileri gelecek
                studentRow.RelativeItem(1).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"{sequenceNumber}");
                studentRow.RelativeItem(1).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"{studentQuizAnswer.Student.SchoolNumber}");
                studentRow.RelativeItem(3).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"{studentQuizAnswer.Student.Name}");
                studentRow.RelativeItem(3).Border(0.25f, Unit.Millimetre).AlignMiddle().AlignCenter().Text($"{studentQuizAnswer.Student.SurName}");
            });

            rst.RelativeItem(9).Row(studentScore =>
            {
                foreach (QuestionScore questionScore in studentQuizAnswer.ScorePerQuestions)// soru sayısı kadar
                    studentScore.RelativeItem(1).Background(AnalsysPageV1MicroCalc.ScoreColorForScore(questionScore)).Border(0.25f, Unit.Millimetre).AlignMiddle().AlignCenter().Text($"{questionScore.Score}");

                studentScore.ConstantItem(1.25f, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForTotalScore(studentQuizAnswer.TotalScore)).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"{studentQuizAnswer.TotalScore}");

                NoteStatus(studentScore, studentQuizAnswer.TotalScore, studentQuizAnswer.SpecialStatuses);
            });
        });
    }

    private static void NoteStatus(RowDescriptor studentScore, int totalScore, char[] crayzChar)
    {
        char[] excludedStatuses = ['k', 'g'];
        char[] excludedStatuses2 = ['m', 'r'];

        string status = $" {string.Concat(crayzChar.Where(c => excludedStatuses2.Contains(c)))}".ToUpper();

        if (crayzChar.Any(c => excludedStatuses.Contains(c)))
        {
            studentScore.ConstantItem(2, Unit.Centimetre).Background("FF0000").AlignLeft().AlignMiddle().Row(t => t.ConstantItem(2, Unit.Centimetre).AlignCenter().Text($"Geçersiz: {string.Concat(crayzChar.Where(c => excludedStatuses.Contains(c))).ToUpper()}"));
            return;
        }

        switch (totalScore)
        {
            case >= 85:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Row(t => t.ConstantItem(2, Unit.Centimetre).AlignCenter().Text("Pek İyi" + status));
                break;
            case >= 70 and < 85:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Row(t => t.ConstantItem(2, Unit.Centimetre).AlignCenter().Text("İyi" + status));
                break;
            case >= 60 and < 70:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Row(t => t.ConstantItem(2, Unit.Centimetre).AlignCenter().Text("Orta" + status));
                break;
            case >= 50 and < 60:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Row(t => t.ConstantItem(2, Unit.Centimetre).AlignCenter().Text("Geçer" + status));
                break;
            default:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Row(t => t.ConstantItem(2, Unit.Centimetre).AlignCenter().Text("Geçmez" + status));
                break;
        }
    }
}
