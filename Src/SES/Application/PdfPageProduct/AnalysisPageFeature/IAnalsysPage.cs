using Entity.Entities.Mains;
using QuestPDF.Infrastructure;

namespace Application.PdfPageProduct.AnalysisPageFeature;

public interface IAnalsysPage
{
    IDocument PageGenerate(Analysis analysisHeader);
}
