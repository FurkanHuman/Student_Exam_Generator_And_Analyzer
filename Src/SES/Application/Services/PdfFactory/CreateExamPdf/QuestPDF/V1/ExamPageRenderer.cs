using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Domain.Entities;
using NArchitecture.Core.Security.Entities;
using QuestPDF.Drawing;
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
    {
        ExamInfo examInfo = refExamInfo;

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
        page.DefaultTextStyle(ts => ts.FontFamily("Arial", "Liberation Sans", "DejaVu Sans", "sans-serif").FontSize(11));
        page.MarginTop(1f, Unit.Centimetre);
        page.MarginBottom(1.5f, Unit.Centimetre);
        page.MarginRight(1f, Unit.Centimetre);
        page.MarginLeft(1f, Unit.Centimetre);
        page.Size(PageSizes.A4);
    }

    private static string WatermarkSVG(string text)
    {
        return $@"<svg viewBox=""0 0 200 200"" xmlns=""http://www.w3.org/2000/svg"">
            <text x=""75"" y=""150"" text-anchor=""middle"" dominant-baseline=""middle"" 
                  fill=""#808080"" opacity=""0.1"" font-size=""16"" 
                  transform=""rotate(-45 100 100)"">{System.Security.SecurityElement.Escape(text)}</text>
        </svg>";
    }
}