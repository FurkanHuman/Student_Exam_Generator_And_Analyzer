using Domain.Common;

namespace Domain.Question.ValueObjects;

public sealed record QuestionContent : ValueObject
{
    public string Prompt { get; init; }
    public string? Stem { get; init; }
    public string? ImageUrl { get; init; }

    private QuestionContent()
    {
        Prompt = string.Empty;
    }

    private QuestionContent(string prompt, string? stem, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new DomainException("Question prompt cannot be empty");

        Prompt = prompt.Trim();
        Stem = stem?.Trim();
        ImageUrl = imageUrl?.Trim();
    }

    public static QuestionContent Create(string prompt, string? stem = null, string? imageUrl = null) => new(prompt, stem, imageUrl);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Prompt;
        yield return Stem;
        yield return ImageUrl;
    }
}
