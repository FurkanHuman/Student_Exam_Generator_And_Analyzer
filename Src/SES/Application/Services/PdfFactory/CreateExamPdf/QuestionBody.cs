
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf;
internal static class QuestionBody
{
    private static readonly char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    internal static IContainer QuestionOption(this IContainer container, string[] optionsText, bool isStandardOptionMode)
    {
        container.Column(col =>
        {
            for (int i = 0; i < optionsText.Length; i++)
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
                         .OptChar(Chars[i], !isStandardOptionMode);
                    });

                    row.ConstantItem(145).Element(inner =>
                    {
                        inner.PaddingTop(textPaddingTop, Unit.Point)
                             .Text(optionsText[i])
                             .FontSize(12);
                    });
                });
            }
        });

        return container;
    }

    internal static IContainer OptChar(this IContainer container, char letter, bool isStandardOptionMode)
    {
        string svg = string.Empty;
        switch (isStandardOptionMode)
        {
            case true:
                svg = $@"
                                <svg width=""12"" height=""12"" viewBox=""0 0 12 12"" xmlns=""http://www.w3.org/2000/svg"">
                                  <circle cx=""6"" cy=""6"" r=""4.5"" stroke=""black"" stroke-width=""0.7"" fill=""white"" />
                                  <text x=""50%"" y=""67%"" text-anchor=""middle"" dominant-baseline=""middle"" font-size=""6"" font-family=""Segoe UI, Arial, sans-serif"" fill=""black"">{letter}</text>
                                </svg>";

                break;

            case false:
                svg = $@"
                                <svg width=""12"" height=""12"" viewBox=""0 0 12 12"" xmlns=""http://www.w3.org/2000/svg"">
                                  <text x=""50%"" y=""90%"" text-anchor=""middle"" dominant-baseline=""middle"" font-size=""12"" font-family=""Segoe UI, Arial, sans-serif"" fill=""black"">{letter})</text>
                                </svg>";
                break;
        }
        container.Svg(svg);
        return container;
    }
}
