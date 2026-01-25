using Domain.Common;
using Domain.Question.ValueObjects;

namespace Domain.Question;

public sealed class QuizQuestion : Entity<int>
{
    public QuestionContent Content { get; private set; }
    public QuestionType Type { get; private set; }
    public bool IsAIGenerated { get; private set; }
    public int? PreviousQuestionId { get; private set; }
    public float[]? Embedding { get; private set; }

    private readonly List<QuestionOption> _options = [];
    public IReadOnlyList<QuestionOption> Options => _options;

    private QuizQuestion()
    {
        Content = QuestionContent.Create("Default");
    }

    public static QuizQuestion Create(string prompt,
                                      QuestionType type,
                                      string? stem = null,
                                      string? imageUrl = null,
                                      bool isAIGenerated = false)
    {
        return new QuizQuestion
        {
            Content = QuestionContent.Create(prompt, stem, imageUrl),
            Type = type,
            IsAIGenerated = isAIGenerated
        };
    }

    public void AddOption(string optionText, bool isCorrect)
    {
        QuestionOption option = QuestionOption.Create(Id, optionText, isCorrect);
        _options.Add(option);
    }

    public void UpdateContent(string prompt, string? stem = null, string? imageUrl = null) => Content = QuestionContent.Create(prompt, stem, imageUrl);

    public void LinkToPreviousQuestion(int previousQuestionId)
    {
        if (previousQuestionId == Id)
            throw new DomainException("Cannot link question to itself");
        PreviousQuestionId = previousQuestionId;
    }

    public void SetEmbedding(float[] embedding)
    {
        if (embedding == null || embedding.Length == 0)
            throw new DomainException("Embedding cannot be empty");
        Embedding = embedding;
    }

    public void MarkAsAIGenerated() => IsAIGenerated = true;
}
