﻿using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace App.ExamPage;
public class PageWrittenExamV1 : IPage
{
    //private readonly IDictionary<int, string> ExamLanguage = new Dictionary<int, string>
    //{
    //    { 0, "Puan" },
    //    { 1, "Score" }
    //}; 
    // Todo : Remeber here

    public IDocument FrontPageGenerate(Exam exam)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
                page.Margin(0.5f, Unit.Centimetre);
                page.Size(pageSize: PageSizes.A4);
                page.Header().Height(3, Unit.Centimetre).Row(headerRow =>
                {
                    headerRow.RelativeItem(2).Padding(0.2f, Unit.Centimetre).Column(nametag =>
                    {
                        nametag.Item().DefaultTextStyle(ts => ts.FontSize(11));
                        
                        nametag.Item().AlignCenter().Text(exam.Student.Name);
                        nametag.Item().AlignCenter().Text(exam.Student.SurName);
                        nametag.Item().AlignCenter().Text($"{exam.Student.ClassAge}/{exam.Student.ClassAge}");
                        nametag.Item().AlignCenter().Text(exam.Student.SchoolNumber);
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

                page.Content().PaddingTop(0.4f, Unit.Centimetre).PaddingBottom(0.4f, Unit.Centimetre)
                .Column(column =>
                {
                    column.Spacing(0.4f, Unit.Centimetre);

                    int count = exam.QuizQuestions.Count;
                    for (int i = 0; i < count && i < 10; i++)
                        column.Item().Height(2, Unit.Centimetre).Text($"{i + 1}) {exam.QuizQuestions[i].Question}");
                });

                page.Footer().Height(1, Unit.Centimetre).Row(footerNotes =>
                {
                    footerNotes.RelativeItem(16).Text(exam.FooterNote).FontSize(9);
                    footerNotes.RelativeItem(1).AlignMiddle().AlignCenter().Text(exam.ExamCode).FontSize(10);
                });
            });
        });
    }

    public IDocument BackPageGenerate(Exam exam)
    {

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Margin(0.5f, Unit.Centimetre);
                page.Size(pageSize: PageSizes.A4);

                page.Content().PaddingTop(0.4f, Unit.Centimetre).PaddingBottom(0.4f, Unit.Centimetre)
                .Column(column =>
                {
                    column.Spacing(0.4f, Unit.Centimetre);

                    int count = exam.QuizQuestions.Count;
                    for (int i = 10; i < count && i < 21; i++)
                        column.Item().Height(2, Unit.Centimetre).Text($"{i + 1}) {exam.QuizQuestions[i].Question}");
                });

                page.Footer().Height(1, Unit.Centimetre).Row(footerNotes =>
                {
                    footerNotes.RelativeItem(16).Text(exam.FooterNote).FontSize(9);
                    footerNotes.RelativeItem(1).AlignMiddle().AlignCenter().Text(exam.ExamCode).FontSize(10);
                });
            });
        });
    }
}
