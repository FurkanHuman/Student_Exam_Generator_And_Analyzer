using Domain.Entities;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf;

public interface IQuestPDFTestPageGenerator
{
    IDocument PageGenerate(Exam exam);
}