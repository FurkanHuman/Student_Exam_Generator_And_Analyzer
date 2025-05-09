using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

public class ExamPageRenderer : IExamPageGenerator
{
    private static readonly byte QuestionsPerPage = 6;

    IDocument IQuestPDFTestPageGenerator.PageGenerate(Exam exam, ref ExamInfo examInfo)
    {
        if (exam.QuizQuestions.Count <= QuestionsPerPage)
            return FrontPageGenerate(exam, examInfo);
        return Document.Merge(FrontPageGenerate(exam, examInfo), BackPageGenerate(exam, examInfo));
    }

    public byte[] PageGenerate(Exam exam, ref ExamInfo examInfo)
    {
        if (exam.QuizQuestions.Count <= QuestionsPerPage)
            return FrontPageGenerate(exam, examInfo).GeneratePdf();
        return Document.Merge(FrontPageGenerate(exam, examInfo), BackPageGenerate(exam, examInfo)).GeneratePdf();
    }

    private static IDocument FrontPageGenerate(Exam exam, ExamInfo examInfo) => Document.Create(document =>
    {
        document.Page(page =>
        {
            page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));

            page.MarginTop(2, Unit.Centimetre);
            page.MarginBottom(2, Unit.Centimetre);
            page.MarginRight(2, Unit.Centimetre);
            page.MarginLeft(2.5f, Unit.Centimetre);
            page.Size(pageSize: PageSizes.A4);
            // custom page design  
            page.Background().Svg(WatermarkSVG("v0.0.1-alpha-04 S.E.S"));
            page.ExamHeader(exam, examInfo);
            page.ExamQuestionContent(exam, 0, Math.Min(QuestionsPerPage, exam.QuizQuestions.Count), examInfo);
            page.ExamFooter(exam, examInfo);
        });
    });


    private static IDocument BackPageGenerate(Exam exam, ExamInfo examInfo) => Document.Create(document =>
    {
        document.Page(page =>
        {
            page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));

            page.MarginTop(2, Unit.Centimetre);
            page.MarginBottom(2, Unit.Centimetre);
            page.MarginRight(2, Unit.Centimetre);
            page.MarginLeft(2.5f, Unit.Centimetre);

            page.Size(pageSize: PageSizes.A4);
            // custom page design
            page.Background().Svg(WatermarkSVG("v0.0.1-alpha-04 S.E.S"));
            page.ExamQuestionContent(exam, QuestionsPerPage, Math.Min(QuestionsPerPage * 2, exam.QuizQuestions.Count), examInfo);
            page.ExamFooter(exam, examInfo);
        });
    });

    private static string WatermarkSVG(string text)
    {
        return @$"<svg viewBox=""0 0 200 200"" xmlns=""http://www.w3.org/2000/svg""><text x=""100"" y=""100"" text-anchor=""middle"" dominant-baseline=""middle"" fill=""gray"" opacity="".3"" font-family=""Arial"" font-size=""16"" transform=""rotate(-45 100 100)"">{text}</text></svg>";
    }
}
