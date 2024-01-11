using Entity.Entities.Mains;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.ExamPageFeature;

public interface IExamPage
{
    IDocument BackPageGenerate(Exam exam);
    IDocument BackPageWithImageGenerate(Exam exam);
    IDocument FrontPageGenerate(Exam exam);
    IDocument FrontPageWithImageGenerate(Exam exam);
}