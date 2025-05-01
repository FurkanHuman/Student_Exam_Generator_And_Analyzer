using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Domain.Entities;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

public interface IQuestPDFTestPageGenerator
{
    IDocument PageGenerate(Exam exam, ref ExamInfo examInfo);
}