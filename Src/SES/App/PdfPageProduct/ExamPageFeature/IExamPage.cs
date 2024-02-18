using Entity.Entities.Mains;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.ExamPageFeature;

public interface IExamPage
{
    IDocument PageGenerate(Exam exam);
}