using Entity.Entities.Mains;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace App.PdfPageProduct.ExamPageFeature.V1;

internal static class ExamPageV1Content
{
    internal static void QuestionContent(Exam exam, PageDescriptor page, int queryStart, int queryEnd)
    {
        page.Content().PaddingTop(0.4f, Unit.Centimetre).PaddingBottom(0.4f, Unit.Centimetre).Column(questionContentColumn =>
        {
            questionContentColumn.Spacing(0.4f, Unit.Centimetre);

            int count = exam.QuizQuestions.Count;
            for (int i = 0; i < count && queryStart < queryEnd; i++)
                QuestionBody(exam, questionContentColumn, i);
            questionContentColumn.Item().PageBreak();
        });
    }


    private static void QuestionBody(Exam exam, ColumnDescriptor questionContentColumn, int index)
    {
        questionContentColumn.Item().Row(questionContentColumnRow =>
        {
            if (exam.QuizQuestions[index].QuestionImage != null)
            {
                questionContentColumnRow.RelativeItem(2).AlignMiddle().AlignCenter().MaxHeight(2.5F, Unit.Centimetre).MaxWidth(5, Unit.Centimetre).Image(filePath: exam.QuizQuestions[index].QuestionImage).FitArea().WithCompressionQuality(ImageCompressionQuality.Best);

                questionContentColumnRow.RelativeItem(0.2f);
                questionContentColumnRow.RelativeItem(14).Height(2, Unit.Centimetre).AlignMiddle().AlignLeft().Text($"{index + 1}) " + exam.QuizQuestions[index].Question);
            }
            else
                questionContentColumnRow.RelativeItem().Height(2, Unit.Centimetre).AlignMiddle().AlignLeft().Text($"{index + 1}) " + exam.QuizQuestions[index].Question);
        });
    }
}
