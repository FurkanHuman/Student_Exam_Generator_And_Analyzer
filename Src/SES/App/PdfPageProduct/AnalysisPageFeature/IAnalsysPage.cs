using Entity.Entities.Mains;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.AnalysisPageFeature;

public interface IAnalsysPage
{
    IDocument PageGenerate(AnalysisHeader analysisHeader);
}
