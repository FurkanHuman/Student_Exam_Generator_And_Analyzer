using Entity.Entities.Mains;
using QuestPDF.Infrastructure;

namespace App.ExamPage;

public interface IPage
{
    IDocument BackPageGenerate(Exam exam);
    IDocument FrontPageGenerate(Exam exam);
}