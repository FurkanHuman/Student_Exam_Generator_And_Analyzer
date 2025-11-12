using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

internal static class ExamPageContent
{
    internal static void ExamQuestionContent(this PageDescriptor page, Exam exam, ExamInfo examInfo, ImageService.IImageServices imageServices)
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
            multi.Spacing(20);

            multi.Spacer().AlignCenter().LineVertical(2).LineColor(Colors.Black);
            multi.BalanceHeight(false);

            multi.Content().Column(column =>
            {
                column.Spacing(10);

                for (int i = 0; i < exam.QuizQuestions.Count; i++)
                {
                    column.Item()
                          .ShowEntire()
                          .PaddingBottom(10)
                          .QuestionBody(exam, i, examInfo, (int)seed, imageServices);
                }
            });
        });
    }

    private static IContainer QuestionBody(this IContainer container, Exam exam, int index, ExamInfo examInfo, int seed, ImageService.IImageServices imageServices)
    {
        container.Row(row =>
        {
            row.Spacing(10);
            row.AutoItem().AlignTop().Text($"{index + 1})");
            row.RelativeItem(1).QuizQuestion(exam.QuizQuestions[index], examInfo, seed, imageServices);
        });

        return container;
    }
}