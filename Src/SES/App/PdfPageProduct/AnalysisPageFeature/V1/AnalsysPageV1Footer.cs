using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.AnalysisPageFeature.V1;

internal static class AnalsysPageV1Footer
{
    internal static void FooterTeacherAndPrincipal(AnalysisHeader analysisHeader, ColumnDescriptor fC)
    {
        fC.Item().PaddingBottom(1, Unit.Centimetre).AlignTop().PaddingHorizontal(2, Unit.Centimetre).DefaultTextStyle(f => f.FontSize(14)).Row(ti =>
        {
            ti.RelativeItem().AlignCenter().AlignLeft().Column(t =>
            {
                t.Item().AlignCenter().Text($"İmza");
                t.Item().AlignCenter().Text($"{analysisHeader.Teacher.Name} {analysisHeader.Teacher.SurName}");
                t.Item().AlignCenter().Text($"Öğretmen");

            });
            ti.RelativeItem().AlignCenter().AlignRight().Column(p =>
            {
                p.Item().AlignCenter().Text($"İmza");
                p.Item().AlignCenter().Text($"{analysisHeader.Principal.Name} {analysisHeader.Principal.SurName}"); ;
                p.Item().AlignCenter().Text($"Okul Müdürü");
            });
        });
    }
}
