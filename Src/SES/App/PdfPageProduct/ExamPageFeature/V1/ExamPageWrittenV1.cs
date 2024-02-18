using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.ExamPageFeature.V1;
public class ExamPageWrittenV1 : IExamPage
{
    //private readonly IDictionary<int, string> ExamLanguage = new Dictionary<int, string>
    //{
    //    { 0, "Puan" },
    //    { 1, "Score" }
    //}; 
    // Todo : Remeber here

    public IDocument PageGenerate(Exam exam)
    {
        if (exam.QuizQuestions.Count <= 10)
            return FrontPageGenerate(exam);
        return Document.Merge(FrontPageGenerate(exam), BackPageGenerate(exam));
    }

    private static IDocument FrontPageGenerate(Exam exam)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial).Fallback(x => x.FontFamily(Fonts.Tahoma)));
                page.Margin(0.5f, Unit.Centimetre);
                page.Size(pageSize: PageSizes.A4);
                ExamPageV1Header.ExamHeader(exam, page);
                ExamPageV1Content.QuestionContent(exam, page, 0, 10);

                page.Footer().Height(1, Unit.Centimetre).Row(footerNotes =>
                {
                    footerNotes.RelativeItem(16).Text(exam.FooterNote).FontSize(9);
                    footerNotes.RelativeItem(1).AlignMiddle().AlignCenter().Text(exam.ExamCode).FontSize(10);
                });
            });
        });
    }

    private static IDocument BackPageGenerate(Exam exam) => Document.Create(document =>
    {
        document.Page(page =>
        {
            page.DefaultTextStyle(ts => ts.FontFamily(Fonts.Arial).Fallback(x => x.FontFamily(Fonts.Tahoma)));
            page.Margin(0.5f, Unit.Centimetre);
            page.Size(pageSize: PageSizes.A4);
            ExamPageV1Content.QuestionContent(exam, page, 11, 21);
            ExamPageV1Footer.Footer(exam, page);
        });
    });
}
