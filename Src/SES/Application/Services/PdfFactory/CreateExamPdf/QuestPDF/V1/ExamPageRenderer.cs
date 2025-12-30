using Application.Services.ImageService;
using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

public class ExamPageRenderer : IExamPageGenerator
{
    private readonly IImageService _imageServices;

    public ExamPageRenderer(IImageService imageServices)
    {
        _imageServices = imageServices;
    }

    IDocument IQuestPDFTestPageGenerator.PageGenerate(Exam exam, ref ExamInfo examInfo)
    {
        return GenerateDocument(exam, ref examInfo);
    }

    public byte[] PageGenerate(Exam exam, ref ExamInfo examInfo)
    {
        return GenerateDocument(exam, ref examInfo).GeneratePdf();
    }

    private Document GenerateDocument(Exam exam, ref ExamInfo refExamInfo)
    {
        ExamInfo examInfo = refExamInfo;

        const byte MaxRetryCount = 3;

        for (byte retryCount = 0; retryCount < MaxRetryCount; retryCount++)
        {
            try
            {
                return DrawExamPage(exam, examInfo);
            }

            catch (Exception ex) when (ex is DocumentDrawingException || ex is DocumentComposeException)
            {
                return HandleDocumentException($"Document Layout Error: {ex.Message}");
            }

            catch (Exception ex)
            {
                exam.ExamTrackingCode = QuizQuestionHelpers.GenerateBase32String();

                if (retryCount == MaxRetryCount - 1)
                    return HandleDocumentException($"Document Retry Failure after {MaxRetryCount} attempts. Last error: {ex.Message}");
            }
        }

        return HandleDocumentException("Unexpected document generation failure.");
    }

    private Document DrawExamPage(Exam exam, ExamInfo examInfo)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                PageSettings(page);

                page.Background().ShowIf(examInfo.ShowWatermark).Svg(WatermarkSVG("v0.0.1-alpha-09 S.E.S"));

                page.ExamHeader(exam, examInfo);

                page.ExamQuestionContent(exam, examInfo, _imageServices);

                page.ExamFooter(exam, examInfo);
            });
        });
    }

    private static Document HandleDocumentException(string errorMessage)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                PageSettings(page);
                page.Content().AlignCenter().AlignMiddle().Text(errorMessage).FontSize(14).FontColor(Colors.Red.Medium);
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