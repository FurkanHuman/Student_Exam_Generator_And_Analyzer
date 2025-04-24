
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfFactory.CreateExamPdf;
internal static class QuestionBody
{

    private static readonly char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    internal static IContainer QuestionOption(this IContainer container, string[] optionsText, bool isStandardOptionMode)
    {
        switch (isStandardOptionMode)
        {
            case true:
                {
                    container.Column(col =>
                    {
                        for (int i = 0; i < optionsText.Length; i++)
                        {
                            col.Item().Padding(2.5f, Unit.Point).Text(txt =>
                            {
                                txt.Element().PaddingBottom(-5.75f, Unit.Point).Height(15, Unit.Point).OptChar(Chars[i], false);
                                txt.Span("  ");
                                txt.Justify();
                                txt.Span(optionsText[i]).FontSize(11);
                            });
                        }
                    });
                    break;
                }

            case false:
                {
                    container.Column(col =>
                    {
                        for (int i = 0; i < optionsText.Length; i++)
                        {
                            col.Item().Padding(2.5f, Unit.Point).Text(txt =>
                            {
                                txt.Element().PaddingBottom(-5.75f, Unit.Point).Height(20, Unit.Point).OptChar(Chars[i], true);
                                txt.Span("   ");
                                txt.Justify();
                                txt.Span(optionsText[i]).FontSize(11);
                            });
                        }
                    });
                    break;
                }

        }

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
                                  <text x=""50%"" y=""65%"" text-anchor=""middle"" dominant-baseline=""middle"" font-size=""6"" font-family=""Segoe UI, Arial, sans-serif"" fill=""black"">{letter}</text>
                                </svg>";

                break;

            case false:
                svg = $@"
                                <svg width=""12"" height=""12"" viewBox=""0 0 12 12"" xmlns=""http://www.w3.org/2000/svg"">
                                  <text x=""50%"" y=""65%"" text-anchor=""middle"" dominant-baseline=""middle"" font-size=""12"" font-family=""Segoe UI, Arial, sans-serif"" fill=""black"">{letter})</text>
                                </svg>";
                break;
        }
        container.Svg(svg);
        return container;
    }
}
