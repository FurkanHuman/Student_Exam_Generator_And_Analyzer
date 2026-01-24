using Domain.Common;

namespace Domain.Question;

public sealed class QuestionOption : Entity<Guid>
{
    public int QuizQuestionId { get; private set; }
    public string OptionText { get; private set; }
    public bool IsCorrect { get; private set; }

    private QuestionOption()
    {
        OptionText = string.Empty;
    }

    internal static QuestionOption Create(int questionId, string optionText, bool isCorrect)
    {
        if (string.IsNullOrWhiteSpace(optionText))
            throw new DomainException("Option text cannot be empty");

        return new QuestionOption
        {
            Id = Guid.NewGuid(),
            QuizQuestionId = questionId,
            OptionText = optionText.Trim(),
            IsCorrect = isCorrect
        };
    }

    public void MarkAsCorrect() => IsCorrect = true;
    public void MarkAsIncorrect() => IsCorrect = false;
}
