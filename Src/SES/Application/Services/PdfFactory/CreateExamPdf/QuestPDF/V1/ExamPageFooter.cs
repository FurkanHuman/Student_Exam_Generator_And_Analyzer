using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using QuestPDF.Fluent;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

internal static class ExamPageFooter
{
    internal static void ExamFooter(this PageDescriptor page, Exam exam, ExamInfo examInfo)
    {
        page.Footer().Column(footerNotes =>
        {
            string examAuthor = $"{exam.ExamAuthor.Personel.Name} {exam.ExamAuthor.Personel.SurName}";
            footerNotes.Item().AlignCenter().Text(examAuthor).FontSize(10);
            footerNotes.Item().AlignCenter().Text(exam.FooterNote).FontSize(10);
            footerNotes.Item().AlignCenter().Text(QuizQuestionHelpers.InsertDashInString(exam.ExamTrackingCode)).FontSize(7);
        });
    }
}