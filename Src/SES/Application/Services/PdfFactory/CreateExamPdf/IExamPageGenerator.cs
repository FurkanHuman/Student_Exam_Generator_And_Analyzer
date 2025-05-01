using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;
using Domain.Entities;

namespace Application.Services.PdfFactory.CreateExamPdf;

public interface IExamPageGenerator: IQuestPDFTestPageGenerator
{
    byte[] PageGenerate(Exam exam, ref ExamInfo examInfo);
}
