using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF;
public class ExamPageRenderer : IExamPageGenerator, IQuestPDFTestPageGenerator
{
    private static readonly byte questionsPerPage = 6;

    // this code is for testing purposes only the main code is in the QuestPDF folder and IQuestPDFTestPageGenerator 
    IDocument IQuestPDFTestPageGenerator.PageGenerate(Exam exam)
    {
        if (exam.QuizQuestions.Count <= questionsPerPage)
            return FrontPageGenerate(exam);
        return Document.Merge(FrontPageGenerate(exam), BackPageGenerate(exam));
    }

    public byte[] PageGenerate(Exam exam)
    {
        if (exam.QuizQuestions.Count <= questionsPerPage)
            return FrontPageGenerate(exam).GeneratePdf();
        return Document.Merge(FrontPageGenerate(exam), BackPageGenerate(exam)).GeneratePdf();
    }

    private static IDocument FrontPageGenerate(Exam exam)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
                page.Margin(0.5f, Unit.Centimetre);
                page.Size(pageSize: PageSizes.A4);
                // custom page design
                page.ExamHeader(exam);
                page.ExamQuestionContent(exam, 0, Math.Min(questionsPerPage, exam.QuizQuestions.Count));
                page.ExamFooter(exam);
            });
        });
    }

    private static IDocument BackPageGenerate(Exam exam) => Document.Create(document =>
    {
        document.Page(page =>
        {
            page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
            page.Margin(0.5f, Unit.Centimetre);
            page.Size(pageSize: PageSizes.A4);
            // custom page design
            page.ExamQuestionContent(exam, questionsPerPage, Math.Min((questionsPerPage * 2), exam.QuizQuestions.Count));
            page.ExamFooter(exam);
        });
    });

}
