namespace Domain.Enums;

/// <summary>
/// Represents the evaluation status of an exam.
/// </summary>
public enum ExamEvaluationStatus : byte
{
    /// <summary>
    /// Exam has not been evaluated yet.
    /// </summary>
    NotEvaluated = 0,

    /// <summary>
    /// Exam has been evaluated and finalized.
    /// </summary>
    Evaluated = 1,

    /// <summary>
    /// Exam excused due to an officially approved health report.
    /// </summary>
    ExcusedWithReport = 2,

    /// <summary>
    /// Exam excused due to other officially accepted excuses (non-health related).
    /// </summary>
    Excused = 3,

    /// <summary>
    /// Exam marked invalid due to disciplinary or procedural violations.
    /// </summary>
    Invalid = 4
}
