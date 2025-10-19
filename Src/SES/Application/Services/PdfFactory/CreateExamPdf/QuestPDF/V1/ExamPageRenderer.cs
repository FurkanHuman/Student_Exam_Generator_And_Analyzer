using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

public class ExamPageRenderer : IExamPageGenerator
{
    IDocument IQuestPDFTestPageGenerator.PageGenerate(Exam exam, ref ExamInfo examInfo)
    {
        return GenerateDocument(exam, ref examInfo);
    }

    public byte[] PageGenerate(Exam exam, ref ExamInfo examInfo)
    {
        return GenerateDocument(exam, ref examInfo).GeneratePdf();
    }

    private static Document GenerateDocument(Exam exam, ref ExamInfo refExamInfo)
    {ExamInfo examInfo = refExamInfo;

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                PageSettings(page); 
                
                page.Background().ShowIf(examInfo.ShowWatermark).Svg(WatermarkSVG("v0.0.1-alpha-09 S.E.S"));

                page.ExamHeader(exam, examInfo);

                page.ExamQuestionContent(exam, examInfo);
            
                page.ExamFooter(exam, examInfo);
            });
        });
    }

    private static void PageSettings(PageDescriptor page)
    {
        page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial));
        page.MarginTop(2, Unit.Centimetre);
        page.MarginBottom(2, Unit.Centimetre);
        page.MarginRight(2, Unit.Centimetre);
        page.MarginLeft(2.5f, Unit.Centimetre);
        page.Size(PageSizes.A4);
    }

    private static string WatermarkSVG(string text)
    {
        return @$"<svg viewBox=""0 0 200 200"" xmlns=""http://www.w3.org/2000/svg""><text x=""100"" y=""100"" text-anchor=""middle"" dominant-baseline=""middle"" fill=""gray"" opacity="".3"" font-family=""Arial"" font-size=""16"" transform=""rotate(-45 100 100)"">{text}</text></svg>";
    }
}