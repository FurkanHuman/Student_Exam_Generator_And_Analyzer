using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

internal static class ExamPageContent
{
    internal static void ExamQuestionContent(this PageDescriptor page, Exam exam, ExamInfo examInfo)
    {
        IList<QuizQuestion> quizQuestions = exam.QuizQuestions;

        uint seed = QuizQuestionHelpers.DecodeBase32String(exam.ExamCode);

        QuizQuestionHelpers.ShuffleQuizQuestions(ref quizQuestions, ref examInfo, (int)seed);
        QuizQuestionHelpers.ShuffleAllQuestionOptions(ref quizQuestions, examInfo, (int)seed);
        QuizQuestionHelpers.OrderQuizQuestions(quizQuestions, ref examInfo, (int)seed);

        exam.QuizQuestions = quizQuestions;

        page.Content().MultiColumn(multi =>
        {
            multi.Columns(2);
            multi.Spacing(10);

            multi.Content().Column(column =>
            {
                column.Spacing(20);

                for (int i = 0; i < exam.QuizQuestions.Count; i++)
                    column.Item()
                          .ShowEntire()
                          .PaddingBottom(20)
                          .QuestionBody(exam, i, examInfo, (int)seed);
            });
        });
    }

    private static IContainer QuestionBody(this IContainer container, Exam exam, int index, ExamInfo examInfo, int seed)
    {
        container.Row(row =>
        {
            row.Spacing(10);
            row.AutoItem().AlignMiddle().AlignTop().Text($"{index + 1})");
            row.RelativeItem(1).AlignMiddle().QuizQuestion(exam.QuizQuestions[index], examInfo, seed);
        });

        return container;
    }
}
