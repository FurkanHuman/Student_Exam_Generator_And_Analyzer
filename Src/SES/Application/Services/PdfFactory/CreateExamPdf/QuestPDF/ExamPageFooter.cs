using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF;

internal static class ExamPageFooter
{
    internal static PageDescriptor ExamFooter(this PageDescriptor page, Exam exam, ExamInfo examInfo)
    {
        page.Footer().Height(0.7f, Unit.Centimetre).Column(footerNotes =>
        {
            footerNotes.Item().AlignCenter().Text(exam.FooterNote).FontSize(9);
            footerNotes.Item().AlignCenter().Text(QuizQuestionHelpers.InsertDashInString(exam.ExamCode)).FontSize(7);
        });
        return page;
    }
}
