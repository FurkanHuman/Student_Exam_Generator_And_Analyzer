using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.PdfPageProduct.AnalysisPageFeature.V1;

internal static class AnalsysPageV1GraphicalAnalsys
{
    internal static void DravClassgraphicalNoteAnalsys(ColumnDescriptor generalClassInfoForStudent, AnalysisHeader analysisHeader, char[] crayzChar)
    {
        generalClassInfoForStudent.Item().Background(Colors.Grey.Lighten1).PaddingTop(0.5F, Unit.Centimetre).PaddingLeft(0.5f, Unit.Centimetre).PaddingRight(0.5f, Unit.Centimetre).DefaultTextStyle(ts => ts.FontSize(9)).Border(0.5f, Unit.Millimetre).Row(rst =>
        {
            rst.RelativeItem().Column(fC =>
            {
                // grafik 1 : n kadar soruya verilen n kadar sorunun puan ortalamsı sütun grafiği.y = puan ortalaması.x = n kadar soru
                List<double> averageScores = AnalsysPageV1MicroCalc.AverageScoresBarGraph(analysisHeader, crayzChar);
                byte[] png = GenGraph.LabeledQuestScoreBar(values: averageScores.ToArray());
                fC.Item().AlignCenter().Width(8.5f, Unit.Centimetre).Height(8.5f, Unit.Centimetre).Border(1).Image(png);
            });
            rst.RelativeItem().Column(fC =>
            {
                //grafik 3 : öğrencilerin sınıf başarı grubu grafiği.N kadar aralık üzerinden.
                int[] scoresOfClass = AnalsysPageV1MicroCalc.AverageScoresPieGraph(analysisHeader, crayzChar).ToArray();
                byte[] png = GenGraph.LabeledClassStudendSegmentPie(scoresOfClass);
                fC.Item().AlignRight().Width(8.5f, Unit.Centimetre).Height(8.5f, Unit.Centimetre).Border(1).Image(png);
            });
        });
    }
}
