namespace Domain.StudentAnswer;

public enum ExamEvaluationStatus : byte
{
    NotEvaluated = 0,
    Evaluated = 1,
    ExcusedWithReport = 2,
    Excused = 3,
    Invalid = 4
}
