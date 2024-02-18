using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.ExamPageFeature.V1;

internal static class ExamPageV1Header
{
    internal static void ExamHeader(Exam exam, PageDescriptor page)
    {
        page.Header().Height(3, Unit.Centimetre).Row(headerRow =>
        {
            headerRow.RelativeItem(2).Padding(0.2f, Unit.Centimetre).Column(nametag =>
            {
                nametag.Item().DefaultTextStyle(ts => ts.FontSize(9));

                nametag.Item().AlignCenter().Text(exam.Student.Name);
                nametag.Item().AlignCenter().Text(exam.Student.SurName);
                nametag.Item().AlignCenter().Text($"{exam.Student.ClassAge}/{exam.Student.ClassBranch} | {exam.Student.SchoolNumber}");

            });

            headerRow.RelativeItem(5).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
            {
                generalInfo.Item().DefaultTextStyle(ts => ts.FontSize(13));

                generalInfo.Item().AlignCenter().Text(exam.ExamSemesterYear);
                generalInfo.Item().AlignCenter().Text(exam.School.Name);
                generalInfo.Item().AlignCenter().Text(exam.LessonName);
                generalInfo.Item().AlignCenter().Text($"{exam.Semester} {exam.SemesterSesion}").FontSize(12);
            });

            headerRow.RelativeItem(1.5f).Padding(0.2f, Unit.Centimetre).Column(examSummary =>
            {
                examSummary.Item().AlignCenter().Text(exam.ExamCode).FontSize(10);
                examSummary.Item().AlignCenter().Text("Puan").Underline().FontSize(14); // Note: ingilizce sonra gelecek
                examSummary.Item().AlignCenter().Text("").FontSize(25); // note : boş çünkü otomatik analizci yapılmadı.
            });
        });
    }
}
