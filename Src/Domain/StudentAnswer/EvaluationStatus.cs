namespace Domain.StudentAnswer;

public enum EvaluationStatus : byte
{
    NotEvaluated = 0,
    Empty = 1,
    InvalidFormat = 2,
    IrrelevantAnswer = 3,
    ManualReviewRequired = 4,
    SuspectedPlagiarism = 5,
    ConfirmedPlagiarism = 6,
    Incorrect = 7,
    PartiallyCorrect = 8,
    Correct = 9
}
