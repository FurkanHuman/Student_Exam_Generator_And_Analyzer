using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

internal static class ExamPageFooter
{
    internal static PageDescriptor ExamFooter(this PageDescriptor page, Exam exam, ExamInfo examInfo)
    {
        page.Footer().Height(1.3f, Unit.Centimetre).Column(footerNotes =>
        {
            string examAuthor = $"{exam.ExamAuthor.Personel.Name} {exam.ExamAuthor.Personel.SurName}";
            footerNotes.Item().AlignCenter().Text(examAuthor).FontSize(12);
            footerNotes.Item().AlignCenter().Text(exam.FooterNote).FontSize(12);    
            footerNotes.Item().AlignCenter().Text(QuizQuestionHelpers.InsertDashInString(exam.ExamCode)).FontSize(7);
        });
        return page;
    }
}
