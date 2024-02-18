using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.AnalysisPageFeature.V1;

internal class AnalsysPageV1ReferenceTable
{
    internal static void RefTable(AnalysisHeader analysisHeader, ColumnDescriptor generalClassInfoForStudent, char[] crayzChars)
    {
        generalClassInfoForStudent.Item().Background(Colors.White).PaddingHorizontal(0.25f, Unit.Centimetre).PaddingTop(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).AlignCenter().Text("Kazanımlar Tablosu");

        for (int i = 0; i < analysisHeader.RefScorePerQuestions.Length; i++)
        {
            double stps = AnalsysPageV1MicroCalc.CalculateClassAveragesPerQuestion(analysisHeader, i, crayzChars);

            generalClassInfoForStudent.Item().Background(Colors.White).PaddingHorizontal(0.25f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(refRow =>
            {
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Border(0.25f, Unit.Millimetre).Text($"#{i + 1}");
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Border(0.25f, Unit.Millimetre).Text($"{analysisHeader.Benefits[i].ReferenceBenefitNumber}");
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Border(0.25f, Unit.Millimetre).Text($"{analysisHeader.Benefits[i].ReferenceBenefitComments}");
                refRow.RelativeItem().PaddingTop(0.1F, Unit.Centimetre).Background(AnalsysPageV1MicroCalc.ScoreColorForScore(stps, analysisHeader.RefScorePerQuestions[i])).Border(0.25f, Unit.Millimetre).Text($"Puan: {Math.Round(stps, 1)}/{analysisHeader.RefScorePerQuestions[i]} {RetunedRef(analysisHeader.RefScorePerQuestions[i], stps)}");

                static string RetunedRef(int refNote, double note)
                {
                    if (note < refNote / 2)
                        return "(Tekrarlanacak)";
                    return "";
                }
            });
        }
    }
}
