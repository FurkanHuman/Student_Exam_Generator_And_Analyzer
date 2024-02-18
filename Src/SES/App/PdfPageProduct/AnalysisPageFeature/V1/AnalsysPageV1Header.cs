using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.AnalysisPageFeature.V1;

internal static class AnalsysPageV1Header
{
    internal static void SchoolAndClassInfo(AnalysisHeader analysisHeader, RowDescriptor headerRow)
    {
        headerRow.RelativeItem(1).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
        {
            generalInfo.Item().DefaultTextStyle(ts => ts.FontSize(13));
            generalInfo.Item().AlignCenter().Text(analysisHeader.ExamSemesterYear); // okul dönem yılı
            generalInfo.Item().AlignCenter().Text(analysisHeader.School.Name); // okul adı
            generalInfo.Item().AlignCenter().Text($"{analysisHeader.LessonName}   {analysisHeader.ClassAge}/{analysisHeader.AltClass} - {analysisHeader.Semester} Sınav Analizi").FontSize(13); //ders adı   sınıf yaşı | . dönem analizi
        });
    }
}
