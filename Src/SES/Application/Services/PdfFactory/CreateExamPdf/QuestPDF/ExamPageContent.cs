using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF;

internal static class ExamPageContent
{

    internal static PageDescriptor ExamQuestionContent(this PageDescriptor page, Exam exam, int queryStart, int queryEnd)
    {
        page.Content().MultiColumn(multi =>
        {
            multi.Columns(2);
            multi.Spacing(15);


            multi.Content().Column(column =>
            {
                column.Spacing(20);

                for (int i = queryStart; i < queryEnd && i < exam.QuizQuestions.Count; i++)
                    column.Item()
                          .ShowEntire()
                          .PaddingBottom(20)
                          .QuestionBody(exam, i);
            });
        });

        return page;
    }

    private static IContainer QuestionBody(this IContainer container, Exam exam, int index)
    {
        container.Row(row =>
        {
            row.Spacing(10);
            row.AutoItem().AlignMiddle().AlignTop().Text($"{index + 1})");
            row.RelativeItem(1).AlignMiddle().QuizQuestion(exam.QuizQuestions[index]);
        });

        return container;
    }

}