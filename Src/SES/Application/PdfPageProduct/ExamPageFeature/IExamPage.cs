using Entity.Entities.Mains;
using QuestPDF.Infrastructure;

namespace Application.PdfPageProduct.ExamPageFeature;

public interface IExamPage
{
    IDocument PageGenerate(Exam exam);
}