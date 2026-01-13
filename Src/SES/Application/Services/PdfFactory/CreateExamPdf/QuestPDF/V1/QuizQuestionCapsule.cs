using Application.Services.ImageService;
using Application.Services.PdfFactory.CreateExamPdf.Constants;
using Application.Services.PdfFactory.CreateExamPdf.DTOs;
using Application.Services.PdfFactory.CreateExamPdf.Helpers;
using Domain.Entities;
using Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf.QuestPDF.V1;

internal static class QuizQuestionCapsule
{
    private static readonly char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    internal static IContainer QuizQuestion(this IContainer container, QuizQuestion question, ExamInfo examInfo, int seed, IImageService imageServices)
    {
        if (question.QuestionType == QuestionType.FillInTheBlank)
            RenderFillInTheBlankMask(ref question);

        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(async innerCol =>
                {
                    innerCol.Spacing(5);

                    await innerCol.RenderImageArea(question, imageServices);
                    innerCol.RenderSTEM(question);
                    innerCol.RenderPrompt(question, examInfo);
                    innerCol.RenderOptions(question, examInfo, seed);
                });
            });
        });

        return container;
    }

    private static async Task<ColumnDescriptor> RenderImageArea(this ColumnDescriptor innerCol, QuizQuestion question, IImageService imageServices)
    {
        if (!string.IsNullOrEmpty(question.QuestionImageURL))
        {
            (byte[] FileBytes, Dictionary<string, object> Meta) =
               await imageServices.DownloadFileAsync(question.QuestionImageURL, CancellationToken.None);

            if (Meta != null && Meta.Count > 0)
                innerCol.Item().AlignCenter().Image(FileBytes).FitArea().WithCompressionQuality(ImageCompressionQuality.VeryLow);

            else
                innerCol.Item().Text(ExamConstants.Get("ImageNotFound")).FontColor(Colors.Red.Medium);
        }

        return innerCol;
    }

    private static ColumnDescriptor RenderSTEM(this ColumnDescriptor innerCol, QuizQuestion question)
    {
        if (!string.IsNullOrEmpty(question.Stem))
            innerCol.Item().Text(question.Stem ?? string.Empty).FontSize(12).Italic();
        return innerCol;
    }

    private static ColumnDescriptor RenderPrompt(this ColumnDescriptor innerCol, QuizQuestion question, ExamInfo examInfo)
    {
        if (!examInfo.ShowScoreInPrompt)
        {
            innerCol.Item().Text($"{question.Prompt ?? string.Empty}").FontSize(12).Bold();
            return innerCol;
        }

        string scoreText = ExamConstants.Get("score") ?? "puan";
        string examScore = $"\n({question.QuestionScore.Score} {scoreText})";
        innerCol.Item().Text($"{question.Prompt ?? string.Empty}{examScore}").FontSize(12).Bold();

        return innerCol;
    }

    private static ColumnDescriptor RenderOptions(this ColumnDescriptor innerCol, QuizQuestion question, ExamInfo examInfo, int seed)
    {
        switch (question.QuestionType)
        {
            case QuestionType.OpenEnded:
                innerCol.Item().PaddingLeft(10).PaddingTop(10).RenderOpenEndedSolidBox();
                break;

            case QuestionType.MultipleChoice:
                if (question.Options != null && question.Options.Count > 0)
                    innerCol.Item().PaddingLeft(10).PaddingTop(10).RenderQuestionOptions(question.Options, examInfo.IsStandardOptionMode);
                break;

            case QuestionType.TrueFalse:
                if (question.Options != null && question.Options.Count > 0)
                    innerCol.Item().PaddingLeft(10).PaddingTop(10).RenderTrueFalseOptions(question.Options);
                break;

            case QuestionType.Matching:
                if (question.Options != null && question.Options.Count > 0)
                    innerCol.Item().PaddingLeft(10).PaddingTop(10).RenderMatchingPairs(question.Options, seed);
                break;

            case QuestionType.Ordering:
                break;

            default:
                break;
        }
        return innerCol;
    }

    private static IContainer RenderMatchingPairs(this IContainer container, IList<QuestionOption> questionOptions, int seed)
    {
        if (questionOptions == null || questionOptions.Count == 0)
            return container;

        Random rnd = new(seed);

        string[] firstParts = new string[questionOptions.Count];
        string[] secondParts = new string[questionOptions.Count];

        questionOptions = [.. questionOptions.Select(q => (Value: q, Order: rnd.Next()))
                                             .OrderBy(x => x.Order)
                                             .Select(x => x.Value).ToList()];

        for (int i = 0; i < questionOptions.Count; i++)
        {
            string[] splitedString = (questionOptions[i].OptionText ?? string.Empty).Split(',');
            firstParts[i] = splitedString.Length > 0 ? splitedString[0].Trim() : string.Empty;
            secondParts[i] = splitedString.Length > 1 ? splitedString[1].Trim() : string.Empty;
        }

        QuizQuestionHelpers.ShuffleStringArray(ref firstParts, rnd.Next());
        QuizQuestionHelpers.ShuffleStringArray(ref secondParts, rnd.Next());

        container.Column(col =>
        {
            for (int i = 0; i < questionOptions.Count; i++)
            {
                col.Item().PaddingBottom(5, Unit.Point).Row(row =>
                {
                    row.RelativeItem(1).PaddingTop(5).Text($"{Chars[i]}) {firstParts[i]}").FontSize(12);
                    row.RelativeItem(1).PaddingTop(5).Text($"{i + 1}) .... {secondParts[i]}").FontSize(12);
                });
            }
        });

        return container;
    }
    private static void RenderFillInTheBlankMask(ref QuizQuestion question)
    {
        if (question?.Options == null || string.IsNullOrEmpty(question.Stem))
            return;

        HashSet<string> shadowStrings = [.. question.Options
            .Where(o => !string.IsNullOrWhiteSpace(o.OptionText))
            .SelectMany(o => o.OptionText!
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()))
            .Where(s => s.Length > 0)
            .Distinct()];

        foreach (string str in shadowStrings)
        {
            string mask = new('.', Math.Min(str.Length + 3, 50));
            question.Stem = question.Stem.Replace(str, mask);
        }
    }


    private static IContainer RenderQuestionOptions(this IContainer container, IList<QuestionOption> questionOptions, bool isStandardOptionMode)
    {
        if (questionOptions == null || questionOptions.Count == 0)
            return container;

        container.Column(col =>
        {
            for (int i = 0; i < questionOptions.Count; i++)
            {
                float height = isStandardOptionMode ? 15 : 20;
                float paddingBottom = isStandardOptionMode ? -5.5f : -5.75f;
                float textPaddingTop = isStandardOptionMode ? 1.5f : 2.5f;

                col.Item().PaddingBottom(5, Unit.Point).Row(row =>
                {
                    row.CreateOptions(questionOptions, isStandardOptionMode, i, height, paddingBottom, textPaddingTop);
                });
            }
        });

        return container;
    }

    private static RowDescriptor CreateOptions(this RowDescriptor row, IList<QuestionOption> questionOptions, bool isStandardOptionMode, int i, float height, float paddingBottom, float textPaddingTop)
    {
        try
        {
            row.ConstantItem(20, Unit.Point).Element(e =>
            {
                e.PaddingBottom(paddingBottom, Unit.Point)
                 .Height(height, Unit.Point)
                 .RenderOptionMarkerDrawing(Chars[i], !isStandardOptionMode);
            });

            row.RelativeItem().Element(inner =>
            {
                inner.PaddingTop(textPaddingTop, Unit.Point)
                     .Text(questionOptions[i].OptionText ?? string.Empty)
                     .FontSize(12);
            });
        }
        catch
        {
            row.AutoItem().Text($"{Chars[i]}) {questionOptions[i].OptionText ?? string.Empty}").FontSize(12);
        }

        return row;
    }

    private static IContainer RenderOpenEndedSolidBox(this IContainer container)
    {
        container.Border(1.5f).Width(185).MinHeight(80).MaxHeight(100);
        return container;
    }

    private static IContainer RenderTrueFalseOptions(this IContainer container, IList<QuestionOption> questionOptions)
    {
        if (questionOptions == null || questionOptions.Count == 0)
            return container;

        questionOptions = [.. questionOptions.OrderBy(x => x.Id)];

        container.Column(col =>
        {
            foreach (QuestionOption questionOption in questionOptions)
            {
                col.Item().PaddingBottom(5, Unit.Point).Row(row =>
                {
                    row.ConstantItem(30, Unit.Point).Element(e => e.PaddingBottom(-5.5f, Unit.Point).Text("(....)"));
                    row.Spacing(5);
                    row.RelativeItem().Element(inner =>
                    {
                        inner.PaddingTop(1.5f, Unit.Point)
                             .Text(questionOption.OptionText ?? string.Empty)
                             .FontSize(12);
                    });
                });
            }
        });

        return container;
    }

    private static IContainer RenderOptionMarkerDrawing(this IContainer container, char letter, bool isStandardOptionMode)
    {
        try
        {
            string svg = isStandardOptionMode
                    ? $@"<svg width=""12"" height=""12"" viewBox=""0 0 12 12"" xmlns=""http://www.w3.org/2000/svg"">
                          <circle cx=""6"" cy=""6"" r=""4.5"" stroke=""black"" stroke-width=""0.7"" fill=""white"" />
                          <text x=""6"" y=""8"" text-anchor=""middle"" font-size=""6"" fill=""black"">{letter}</text>
                        </svg>"

                    : $@"<svg width=""12"" height=""12"" viewBox=""0 0 12 12"" xmlns=""http://www.w3.org/2000/svg"">
                          <text x=""0"" y=""10"" font-size=""12"" fill=""black"">{letter})</text>
                        </svg>";

            container.Svg(svg);
        }
        catch
        {
            container.Text($"{letter})").FontSize(12);
        }

        return container;
    }
}