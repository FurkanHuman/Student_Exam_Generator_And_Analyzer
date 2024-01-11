using Entity.Entities.Mains;
using HarfBuzzSharp;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ScottPlot;

namespace App.PdfPageProduct.AnalysisPageFeature;

public class AnalsysPageWrittenV1 : IAnalsysPage
{
    public IDocument PageGenerate(AnalysisHeader analysisHeader)
    {
        //r raporlu = hastalık,m= etkinlik,g geçersiz ,kopya, devam +++
        // todo: k, g, başarı ortalamısın'dan dışla. ++
        // todo kaazanım numarası, açıklama, otomatik mesaj, renkledirmme, sağ sol 
        // todo: kazanın durumu eklenecek. formul ref puanın yarısının altında ise otomatik mesaj verilecek, 
        // todo: kazanım tablosu grafiler sayfa 2 de.
        // todo: (Okul Müdürü: ) müdür sağ alt tarfta , sol alt öğretmen adı. ++
        // todo: puan kaç kişi 5 üzerinden ne aldı. tablo, pasta. sınıf 5 kaç aldı. durumu yaz. ++

        char[] CrayzChars = ['k', 'g'];

        return Document.Create(doc =>
        doc.Page(page =>
        {
            page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
            page.Margin(0.5f, Unit.Centimetre);
            page.Size(pageSize: PageSizes.A4);

            page.Header().Height(3.5f, Unit.Centimetre).Row(headerRow =>
            {
                headerRow.RelativeItem(1).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
                {
                    generalInfo.Item().DefaultTextStyle(ts => ts.FontSize(13));
                    generalInfo.Item().AlignCenter().Text(analysisHeader.ExamSemesterYear); // okul dönem yılı
                    generalInfo.Item().AlignCenter().Text(analysisHeader.School.Name); // okul adı
                    generalInfo.Item().AlignCenter().Text($"{analysisHeader.LessonName}   {analysisHeader.ClassAge}/{analysisHeader.AltClass} - {analysisHeader.Semester} Sınav Analizi").FontSize(13); //ders adı   sınıf yaşı | . dönem analizi
                });
            });

            page.Content().Row(cRow =>
            {
                cRow.RelativeItem().Column(generalClassInfoForStudent => // sınıfın giren öğrenci sayısı | girmeyen öğrenci sayısı | başarılı öğrenci sayısı | başarısız öğrenci sayısı
                {
                    StudentPreTop(analysisHeader, generalClassInfoForStudent);
                    TopInfo(generalClassInfoForStudent, analysisHeader.RefScorePerQuestions);
                    int sequence = 1;
                    foreach (StudentQuizAnswer studentQuizAnswer in analysisHeader.StudentQuizAnswers)
                    {
                        StudensAndNotes(generalClassInfoForStudent, studentQuizAnswer, sequence, studentQuizAnswer.SpecialStatuses);
                        sequence++;
                    }

                    ClassScoreSummary(generalClassInfoForStudent, analysisHeader, CrayzChars);
                    DravClassNoteAnalsys(generalClassInfoForStudent, analysisHeader, CrayzChars);
                    RefTable(analysisHeader, generalClassInfoForStudent, CrayzChars);
                });
            });

            page.Footer().Column(fC =>
            {
                FooterTeacherAndPrincipal(analysisHeader, fC);
                fC.Item().AlignBottom().AlignCenter().Text(analysisHeader.FooterNote).FontSize(9).FontFamily(Fonts.Arial);
            });
        }));
    }

    private static void RefTable(AnalysisHeader analysisHeader, ColumnDescriptor generalClassInfoForStudent, char[] crayzChars)
    {
        generalClassInfoForStudent.Item().Background(Colors.White).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).AlignCenter().Text("Kazanımlar Tablosu");

        for (int i = 0; i < analysisHeader.RefScorePerQuestions.Length; i++)
        {
            double stps = CalculateClassAveragesPerQuestion(analysisHeader, i, crayzChars);

            generalClassInfoForStudent.Item().Background(Colors.White).PaddingHorizontal(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(refRow =>
            {
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Border(0.25f, Unit.Millimetre).Text($"#{i + 1}");
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Border(0.25f, Unit.Millimetre).Text($"{analysisHeader.RefBeneFitsNo[i]}");
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Border(0.25f, Unit.Millimetre).Text($"{analysisHeader.RefBeneFitCommets[i]}");
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Background(ScoreColorForScore(stps, analysisHeader.RefScorePerQuestions[i])).Border(0.25f, Unit.Millimetre).Text($"Puan: {Math.Round(stps, 1)}/{analysisHeader.RefScorePerQuestions[i]} {RetunedRef(analysisHeader.RefScorePerQuestions[i], stps)}");

                static string RetunedRef(int refNote, double note)
                {
                    if (note < (refNote / 2))
                        return "(Tekrarlanacak)";
                    return null;
                }
            });
        }
    }

    private static void FooterTeacherAndPrincipal(AnalysisHeader analysisHeader, ColumnDescriptor fC)
    {
        fC.Item().PaddingBottom(1, Unit.Centimetre).AlignTop().PaddingHorizontal(2, Unit.Centimetre).DefaultTextStyle(f => f.FontSize(14)).Row(ti =>
        {
            ti.RelativeItem().AlignCenter().AlignLeft().Text($"{analysisHeader.Teacher.Name} {analysisHeader.Teacher.SurName}");
            ti.RelativeItem().AlignCenter().AlignRight().Text($"{analysisHeader.Principal.Name} {analysisHeader.Principal.SurName}");
        });
    }

    private static void ClassScoreSummary(ColumnDescriptor generalClassInfoForStudent, AnalysisHeader analysisHeader, char[] crayzChar)
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

                css.ConstantItem(3.25f, Unit.Centimetre).Background(ScoreColorForScore(studentSum, refStudentSum)).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"Sınıf Ortalaması: {studentSum}");

            });
        });
        generalClassInfoForStudent.Item().Background(Colors.Pink.Lighten4).PaddingHorizontal(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(rst =>
        {
            rst.RelativeItem(3.5F).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text("Sınıf Başarı Tablosu: ");
            rst.RelativeItem(9).Row(css =>
            {
                List<int> scores = ScoreSum(analysisHeader, crayzChar);

                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(1) \"Geçmez\" : {scores.Where(p => p < 2).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(2) \"Geçer\" : {scores.Where(p => p >= 2 & p < 3).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(3) \"Orta\" : {scores.Where(p => p >= 3 & p < 4).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(4) \"İyi\" : {scores.Where(p => p >= 4 & p < 5).Count()}");
                css.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"(5) \"Pek İyi\" : {scores.Where(p => p >= 5).Count()}");

                css.ConstantItem(3.25f, Unit.Centimetre).Background(ScoreColorForScore(scores.Sum() / scores.Count, 5)).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"Sınıf Ortalaması: {Math.Round((double)scores.Sum() / scores.Count, 2)}");
            });
        });
    }

    private static List<int> ScoreSum(AnalysisHeader analysisHeader, char[] crayzChar)
    {
        int studentCount = analysisHeader.StudentQuizAnswers.Count;
        List<int> scores = new();
        foreach (StudentQuizAnswer studentQuizAnswer in analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))))
        {
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
        }
        return scores;
    }

    private static void QuestionPerDividedStudents(AnalysisHeader analysisHeader, RowDescriptor css, char[] crayzChar)
    {
        for (int i = 0; i < analysisHeader.RefScorePerQuestions.Length; i++)
        {
            double stps = CalculateClassAveragesPerQuestion(analysisHeader, i, crayzChar);
            css.RelativeItem(1).Background(ScoreColorForScore(stps, analysisHeader.RefScorePerQuestions[i])).Border(0.25f, Unit.Millimetre).AlignMiddle().AlignCenter().Text($"{Math.Round(stps, 1)}");
        }
    }
    private static void ScoreDividedStudents(AnalysisHeader analysisHeader, RowDescriptor css, char[] crayzChar)
    {
        for (int i = 0; i < analysisHeader.RefScorePerQuestions.Length; i++)
        {
            double stps = CalculateClassAveragesPerQuestion(analysisHeader, i, crayzChar);
            css.RelativeItem(1).Background(ScoreColorForScore(stps, analysisHeader.RefScorePerQuestions[i])).Border(0.25f, Unit.Millimetre).AlignMiddle().AlignCenter().Text($"{Math.Round(stps, 1)}");
        }
    }

    private static void DravClassNoteAnalsys(ColumnDescriptor generalClassInfoForStudent, AnalysisHeader analysisHeader, char[] crayzChar)
    {

        generalClassInfoForStudent.Item().Background(Colors.Grey.Lighten1).PaddingTop(0.5F, Unit.Centimetre).PaddingLeft(0.5f, Unit.Centimetre).PaddingRight(0.5f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(rst =>
        {
            rst.RelativeItem().Column(fC =>
            {
                // grafik 1 : n kadar soruya verilen n kadar sorunun puan ortalamsı sütun grafiği.y = puan ortalaması.x = n kadar soru
                List<double> averageScores = AverageScoresBarGraph(analysisHeader, crayzChar);
                byte[] png = GenGraph.LabeledQuestScoreBar(values: averageScores.ToArray());
                fC.Item().AlignLeft().Width(8.5f, Unit.Centimetre).Height(8.5f, Unit.Centimetre).Border(1).Image(png);
            });
            rst.RelativeItem().Column(fC =>
            {
                //grafik 3 : öğrencilerin sınıf başarı grubu grafiği.N kadar aralık üzerinden.
                double[] scoresOfClass = AverageScoresPieGraph(analysisHeader, crayzChar).ToArray();
                byte[] png = GenGraph.LabeledClassStudendSegmentPie(values: scoresOfClass);
                fC.Item().AlignRight().Width(8.5f, Unit.Centimetre).Height(8.5f, Unit.Centimetre).Border(1).Image(png);
            });
        });
    }


    private static void StudentPreTop(AnalysisHeader analysisHeader, ColumnDescriptor generalClassInfoForStudent)
    {
        generalClassInfoForStudent.Item().Background(Colors.Green.Accent2).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(10)).Row(generalClassInfoForStudentRow =>
        {
            generalClassInfoForStudentRow.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Toplam öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Count}");
            generalClassInfoForStudentRow.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınava girmeyen öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s => s.SpecialStatuses.Contains('g'))}");
            generalClassInfoForStudentRow.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınava Tekrardan Girebilecek öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s => s.SpecialStatuses.Contains('r') | s.SpecialStatuses.Contains('m')).Count()}");
            generalClassInfoForStudentRow.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınavda başarılı öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s => s.TotalScore >= 50 & (!s.SpecialStatuses.Contains('k') | !s.SpecialStatuses.Contains('g'))).Count()}");
            generalClassInfoForStudentRow.RelativeItem().Border(0.05f, Unit.Centimetre).AlignCenter().Text($"Sınavda başarısız öğrenci sayısı: {analysisHeader.StudentQuizAnswers.Where(s => s.TotalScore < 50 | s.SpecialStatuses.Contains('k') | s.SpecialStatuses.Contains('g'))}");
        });
    }

    private static void TopInfo(ColumnDescriptor generalClassInfoForStudent, int[] refScores)
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

    private static void StudensAndNotes(ColumnDescriptor generalClassInfoForStudent, StudentQuizAnswer studentQuizAnswer, int sequenceNumber, char[] crayzChar)
    {
        generalClassInfoForStudent.Item().Background(RowColorChangeForInt(sequenceNumber)).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.1f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(rst =>
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
                    studentScore.RelativeItem(1).Background(ScoreColorForScore(questionScore)).Border(0.25f, Unit.Millimetre).AlignMiddle().AlignCenter().Text($"{questionScore.Score}");

                studentScore.ConstantItem(1.25f, Unit.Centimetre).Background(ScoreColorForTotalScore(studentQuizAnswer.TotalScore)).Border(0.25f, Unit.Millimetre).AlignCenter().AlignMiddle().Text($"{studentQuizAnswer.TotalScore}");

                NoteStatus(studentScore, studentQuizAnswer.TotalScore, studentQuizAnswer.SpecialStatuses);
            });
        });
    }

    private static string RowColorChangeForInt(int sequenceNumber)
    {
        return (sequenceNumber % 2) switch
        {
            0 => Colors.LightBlue.Lighten2,
            _ => Colors.LightBlue.Lighten4,
        };
    }

    private static void NoteStatus(RowDescriptor studentScore, int totalScore, char[] crayzChar)
    {
        char[] excludedStatuses = ['k', 'g'];
        char[] excludedStatuses2 = ['m', 'r'];

        string status = $" {string.Concat(crayzChar.Where(c => excludedStatuses2.Contains(c)))}".ToUpper();

        if (crayzChar.Any(c => excludedStatuses.Contains(c)))
        {
            studentScore.ConstantItem(2, Unit.Centimetre).Background("FF0000").AlignLeft().AlignMiddle().Text($"Geçersiz: {string.Concat(crayzChar.Where(c => excludedStatuses.Contains(c)))}");
            return;
        }

        switch (totalScore)
        {
            case >= 85:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Text("Pek İyi" + status);
                break;
            case >= 70 and < 85:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Text("İyi" + status);
                break;
            case >= 60 and < 70:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Text("Orta" + status);
                break;
            case >= 50 and < 60:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Text("Geçer" + status);
                break;
            default:
                studentScore.ConstantItem(2, Unit.Centimetre).Background(ScoreColorForTotalScore(totalScore)).AlignLeft().AlignMiddle().Text("Geçmez" + status);
                break;
        }
    }

    private static string ScoreColorForScore(QuestionScore questionScore)
    {
        double ratio = (double)questionScore.Score / questionScore.MaxScore;
        return ratio switch
        {
            >= 0.8 => "#00FF00",
            >= 0.6 and < 0.8 => "#C0FF00",
            >= 0.4 and < 0.6 => "#FFF000",
            >= 0.2 and < 0.4 => "#FF8000",
            _ => "#FF1010"
        };
    }
    private static string ScoreColorForScore(double questionScore, int refScore)
    {
        double ratio = (double)questionScore / refScore;
        return ratio switch
        {
            >= 0.8 => "#00FF00",
            >= 0.6 and < 0.8 => "#C0FF00",
            >= 0.4 and < 0.6 => "#FFF000",
            >= 0.2 and < 0.4 => "#FF8000",
            _ => "#FF1010"
        };
    }

    private static string ScoreColorForTotalScore(int totalScore)
    {
        return totalScore switch
        {
            >= 85 => "#00FF00",
            >= 70 and < 84 => "#E0FF00",
            >= 60 and < 70 => "#FFFF00",
            >= 50 and < 60 => "#FF8000",
            _ => "#FF0000"
        };
    }
    private static List<double> AverageScoresPieGraph(AnalysisHeader analysisHeader, char[] crayzChar)
    {
        List<double> averageScores = new();
        int questionCount = analysisHeader.RefScorePerQuestions.Length;

        foreach (StudentQuizAnswer studentAnswer in analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))))
        {
            switch (studentAnswer.TotalScore)
            {
                case >= 85:
                    averageScores.Add(5);
                    break;
                case >= 70 and < 84:
                    averageScores.Add(4);
                    break;
                case >= 60 and < 70:
                    averageScores.Add(3);
                    break;
                case >= 50 and < 60:
                    averageScores.Add(2);
                    break;
                default:
                    averageScores.Add(1);
                    break;
            }
        }
        return averageScores;
    }

    private static List<double> AverageScoresBarGraph(AnalysisHeader analysisHeader, char[] crayzChar)
    {
        List<double> averageScores = new();
        int questionCount = analysisHeader.RefScorePerQuestions.Length;

        for (int j = 0; j < questionCount; j++)
        {
            double totalScore = 0;
            foreach (StudentQuizAnswer quizAnswer in analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))))
                totalScore += quizAnswer.ScorePerQuestions[j].Score;

            double averageScore = totalScore / analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))).Count();
            averageScores.Add(averageScore);
        }
        return averageScores;
    }

    public static double CalculateClassAveragesPerQuestion(AnalysisHeader analysisHeader, int index, char[] crayzChar)
    {
        int totalScore = 0;
        int sdudentCount = analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))).Count();

        foreach (StudentQuizAnswer studentQuizAnswer in analysisHeader.StudentQuizAnswers.Where(s => !crayzChar.Any(c => s.SpecialStatuses.Contains(c))))
            totalScore += studentQuizAnswer.ScorePerQuestions[index].Score;

        return (double)totalScore / sdudentCount;
    }
}
