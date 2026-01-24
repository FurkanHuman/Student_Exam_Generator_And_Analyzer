using Domain.Common;
using Domain.Shared.ValueObjects;

namespace Domain.StudentAnswer;

public sealed class StudentAnswer : Entity<Guid>
{
    public Guid StudentExamAnswerId { get; private set; }
    public int QuizQuestionId { get; private set; }
    public Guid? QuestionOptionId { get; private set; }
    public string? AnswerText { get; private set; }
    public Score? GivenScore { get; private set; }
    public EvaluationStatus Status { get; private set; }

    private StudentAnswer() { }

    public static StudentAnswer CreateWithOption(Guid studentExamAnswerId, int quizQuestionId, Guid questionOptionId)
    {
        return new StudentAnswer
        {
            Id = Guid.NewGuid(),
            StudentExamAnswerId = studentExamAnswerId,
            QuizQuestionId = quizQuestionId,
            QuestionOptionId = questionOptionId,
            Status = EvaluationStatus.NotEvaluated
        };
    }

    public static StudentAnswer CreateWithText(Guid studentExamAnswerId, int quizQuestionId, string answerText)
    {
        if (string.IsNullOrWhiteSpace(answerText))
            throw new DomainException("Answer text cannot be empty");

        return new StudentAnswer
        {
            Id = Guid.NewGuid(),
            StudentExamAnswerId = studentExamAnswerId,
            QuizQuestionId = quizQuestionId,
            AnswerText = answerText.Trim(),
            Status = EvaluationStatus.NotEvaluated
        };
    }

    public static StudentAnswer CreateEmpty(Guid studentExamAnswerId, int quizQuestionId)
    {
        return new StudentAnswer
        {
            Id = Guid.NewGuid(),
            StudentExamAnswerId = studentExamAnswerId,
            QuizQuestionId = quizQuestionId,
            Status = EvaluationStatus.Empty
        };
    }

    public void Evaluate(int score, int maxScore, EvaluationStatus status)
    {
        if (status == EvaluationStatus.NotEvaluated)
            throw new DomainException("Cannot set status to NotEvaluated");

        GivenScore = Score.Create(score, maxScore);
        Status = status;
    }

    public void MarkAsCorrect(int maxScore)
    {
        GivenScore = Score.Create(maxScore, maxScore);
        Status = EvaluationStatus.Correct;
    }

    public void MarkAsIncorrect(int maxScore)
    {
        GivenScore = Score.Zero(maxScore);
        Status = EvaluationStatus.Incorrect;
    }

    public void MarkAsPartiallyCorrect(int score, int maxScore)
    {
        GivenScore = Score.Create(score, maxScore);
        Status = EvaluationStatus.PartiallyCorrect;
    }

    public void RequireManualReview() => Status = EvaluationStatus.ManualReviewRequired;

    public bool IsEvaluated() => Status != EvaluationStatus.NotEvaluated && Status != EvaluationStatus.Empty;
}
