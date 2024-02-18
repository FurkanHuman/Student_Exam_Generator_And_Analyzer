using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.ExamPageFeature.V1;

internal static class ExamPageV1Footer
{
    internal static void Footer(Exam exam, PageDescriptor page)
    {
        page.Footer().Height(1, Unit.Centimetre).Row(footerNotes =>
        {
            footerNotes.RelativeItem(16).Text(exam.FooterNote).FontSize(9);
            footerNotes.RelativeItem(1).AlignMiddle().AlignCenter().Text(exam.ExamCode).FontSize(10);
        });
    }
}
