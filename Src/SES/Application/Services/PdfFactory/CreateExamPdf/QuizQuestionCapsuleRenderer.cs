using Domain.Entities;
using Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf;

internal static class QuizQuestionCapsuleRenderer
{
    private static readonly char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    internal static IContainer QuizQuestion(this IContainer container, QuizQuestion question)
    {
        if (question.QuestionType == QuestionType.FillInTheBlank)
            RenderFillInTheBlankMask(ref question);

        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.ConstantItem(200).PaddingTop(25).Column(col =>
                {
                    col.Spacing(5);
                    if (!string.IsNullOrEmpty(question.QuestionImageURL))
                        col.Item().Image(question.QuestionImageURL).FitArea();

                    if (!string.IsNullOrEmpty(question.QuestionBody))
                        col.Item().Text(question.Question).FontSize(12).Italic().Justify();

                    col.Item().Text(question.QuestionBody).FontSize(12).Bold().Justify();

                    switch (question.QuestionType)
                    {
                        case QuestionType.OpenEnded:
                            col.Item().Row(rw => rw.AutoItem().PaddingLeft(10).PaddingTop(10).RenderOpenEndedSolidBox());
                            break;
                        case QuestionType.MultipleChoice:
                            col.Item().Row(rw => rw.AutoItem().PaddingLeft(10).PaddingTop(10).RenderQuestionOptions(question.Options, true));
                            break;
                        case QuestionType.TrueFalse:
                            col.Item().Row(rw => rw.AutoItem().PaddingLeft(10).MaxHeight(30).RenderTrueFalseDrawing());
                            break;
                        case QuestionType.Matching:
                            col.Item().Row(rw => rw.AutoItem().PaddingLeft(10).PaddingTop(10).RenderMatchingPairs(question.Options));
                            break;
                        case QuestionType.Ordering:
                            break;
                        default:
                            break;
                    }
                });
            });
        });

        return container;
    }

    private static IContainer RenderMatchingPairs(this IContainer container, IList<QuestionOption> questionOptions)
    {
        string[] firstParts = new string[questionOptions.Count];
        string[] secondParts = new string[questionOptions.Count];

        for (int i = 0; i < questionOptions.Count; i++)
        {
            string[] splitedString = questionOptions[i].OptionText!.Split(',');
            firstParts[i] = splitedString[0];
            secondParts[i] = splitedString[1];
        }

        Shuffle(ref firstParts, 0101);
        Shuffle(ref secondParts, 1001);

        container.Column(col =>
        {
            for (int i = 0; i < questionOptions.Count; i++)
                col.Item().PaddingBottom(5, Unit.Point).Row(row =>
                {
                    row.ConstantItem(90).Element(e => e.PaddingTop(5).Text($"{Chars[i]}) {firstParts[i]}").FontSize(12).AlignLeft());
                    row.ConstantItem(90).Element(e => e.PaddingTop(5).Text($"{i + 1}) .... {secondParts[i]}").FontSize(12).AlignLeft());
                });
        });

        return container;
    }

    private static IContainer RenderQuestionOptions(this IContainer container, IList<QuestionOption> questionOptions, bool isStandardOptionMode)
    {
        container.Column(col =>
        {
            for (int i = 0; i < questionOptions.Count; i++)
            {
                float height = isStandardOptionMode ? 15 : 20;
                float paddingBottom = isStandardOptionMode ? -5.5f : -5.75f;
                float textPaddingTop = isStandardOptionMode ? 1.5f : 2.5f;

                col.Item().PaddingBottom(5, Unit.Point).Row(row =>
                {
                    row.ConstantItem(20, Unit.Point).Element(e =>
                    {
                        e.PaddingBottom(paddingBottom, Unit.Point)
                         .Height(height, Unit.Point)
                         .RenderOptionMarkerDrawing(Chars[i], !isStandardOptionMode);
                    });

                    row.ConstantItem(145).Element(inner =>
                    {
                        inner.PaddingTop(textPaddingTop, Unit.Point)
                             .Text(questionOptions[i].OptionText)
                             .FontSize(12);
                    });
                });
            }
        });

        return container;
    }


    private static IContainer RenderOpenEndedSolidBox(this IContainer container)
    {
        container.Border(1.5f).Width(185).Height(75);
        return container;
    }

    private static IContainer RenderOpenEndedDashedBoxDrawing(this IContainer container)
    {
        string svg = "<svg xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"100%\" height=\"100%\" fill=\"none\" stroke=\"#000\" stroke-width=\"2\" stroke-dasharray=\"2\"/></svg>";
        container.Svg(svg);
        return container;
    }
    private static IContainer RenderTrueFalseDrawing(this IContainer container)
    {
        string svg = $"<svg width=\"50\" height=\"35\" xmlns=\"http://www.w3.org/2000/svg\"><g fill=\"none\" stroke=\"#000\"><path stroke-width=\"2\" d=\"m4 10 5 6L21 4\"/><circle cx=\"11\" cy=\"28\" r=\"6\"/></g><g transform=\"translate(30)\" stroke=\"#000\"><path stroke-width=\"2\" d=\"m4 4 14 14m0-14L4 18\"/><circle cx=\"11\" cy=\"28\" r=\"6\" fill=\"none\"/></g></svg>";
        container.Svg(svg);
        return container;
    }

    private static IContainer RenderOptionMarkerDrawing(this IContainer container, char letter, bool isStandardOptionMode)
    {
        string svg = isStandardOptionMode
                ? $@"
                    <svg width=""12"" height=""12"" viewBox=""0 0 12 12"" xmlns=""http://www.w3.org/2000/svg"">
                      <circle cx=""6"" cy=""6"" r=""4.5"" stroke=""black"" stroke-width=""0.7"" fill=""white"" />
                      <text x=""50%"" y=""67%"" text-anchor=""middle"" dominant-baseline=""middle"" font-size=""6"" font-family=""Segoe UI, Arial, sans-serif"" fill=""black"">{letter}</text>
                    </svg>"
                : $@"
                    <svg width=""12"" height=""12"" viewBox=""0 0 12 12"" xmlns=""http://www.w3.org/2000/svg"">
                      <text x=""50%"" y=""90%"" text-anchor=""middle"" dominant-baseline=""middle"" font-size=""12"" font-family=""Segoe UI, Arial, sans-serif"" fill=""black"">{letter})</text>
                    </svg>";

        container.Svg(svg);
        return container;
    }

    private static void RenderFillInTheBlankMask(ref QuizQuestion question)
    {
        List<string> shadowStrings = [];

        foreach (QuestionOption option in question.Options)
            shadowStrings.AddRange(option.OptionText!.Split(','));

        foreach (string str in shadowStrings)
            question.Question = question.Question.Replace(str, new string('.', str.Length + 3));
    }

    private static void Shuffle(ref string[] array, int seed)
    {
        Random rng = new(seed);
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
