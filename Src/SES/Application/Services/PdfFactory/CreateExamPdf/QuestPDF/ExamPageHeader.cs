using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF;

internal static class ExamPageHeader
{
    internal static PageDescriptor ExamHeader(this PageDescriptor page, Exam exam)
    {
        page.Header().Height(3, Unit.Centimetre).Row(headerRow =>
        {
            headerRow.RelativeItem(2).Padding(0.2f, Unit.Centimetre).Column(nametag =>
            {
                nametag.Item().DefaultTextStyle(ts => ts.FontSize(9));

                nametag.Item().AlignCenter().Text(exam.Student.Name);
                nametag.Item().AlignCenter().Text(exam.Student.SurName);
                nametag.Item().AlignCenter().Text($"{exam.Student.StudentClass.ClassAge}/{exam.Student.StudentClass.ClassBranch} | {exam.Student.SchoolNumber}");
            });

            headerRow.RelativeItem(5).Padding(0.2f, Unit.Centimetre).Column(generalInfo =>
            {
                generalInfo.Item().DefaultTextStyle(ts => ts.FontSize(15));

                generalInfo.Item().AlignCenter().Text($"{exam.Semester.BeginSemesterDate.Year}/{exam.Semester.EndSemesterDate.Year}").FontSize(12).Bold();
                generalInfo.Item().AlignCenter().Text(exam.School.Name).Bold();
                generalInfo.Item().AlignCenter().Text($"{exam.Lesson.LessonName}").Bold();
                generalInfo.Item().AlignCenter().Text($"{exam.Student.StudentClass.ClassAge}. Sınıflar ... Dönem ... Yazılı Sınavı");
            });

            headerRow.RelativeItem(1.5f).Padding(0.2f, Unit.Centimetre).Column(examSummary =>
            {
                examSummary.Item().AlignCenter().Text(exam.ExamCode).FontSize(10);
                examSummary.Item().AlignCenter().Text("Puan").Underline().FontSize(14);
                //examSummary.Item().AlignCenter().Text("").FontSize(25); // note : boş çünkü otomatik analizci yapılmadı.
            });
        });
        return page;
    }
}
