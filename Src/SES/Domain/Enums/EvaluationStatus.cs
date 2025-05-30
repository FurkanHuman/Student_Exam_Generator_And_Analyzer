namespace Domain.Enums;

/// <summary>
/// Represents the evaluation status of an answer.
/// </summary>
public enum EvaluationStatus : byte
{
    /// <summary>
    /// Not evaluated yet.
    /// </summary>
    NotEvaluated,

    /// <summary>
    /// The answer is empty.
    /// </summary>
    Empty,

    /// <summary>
    /// The answer has an invalid format or does not meet the required criteria.
    /// </summary>
    InvalidFormat,

    /// <summary>
    /// The answer is irrelevant to the subject.
    /// </summary>
    IrrelevantAnswer,

    /// <summary>
    /// Human review is required, as the AI system is uncertain.
    /// </summary>
    ManualReviewRequired,

    /// <summary>
    /// The answer is suspected of plagiarism by the AI or system.
    /// </summary>
    SuspectedPlagiarism,

    /// <summary>
    /// The answer is confirmed as plagiarized after manual verification.
    /// </summary>
    ConfirmedPlagiarism,

    /// <summary>
    /// The answer is incorrect.
    /// </summary>
    Incorrect,

    /// <summary>
    /// The answer is partially correct.
    /// </summary>
    PartiallyCorrect,

    /// <summary>
    /// The answer is completely correct.
    /// </summary>
    Correct
}
