using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Domain.Entities;

namespace Application.Services.PdfFactory.CreateExamPdf;

public interface IExamPageGenerator
{
    byte[] PageGenerate(Exam exam, ref ExamInfo examInfo);
}
