namespace Domain.Enums;
/// <summary>
/// Represents the origin of the evaluation for a student's answer.
/// </summary>
public enum EvaluationOrigin : byte
{
    /// <summary>
    /// not evaluated yet.
    /// </summary>
    NotEvaluated,

    /// <summary>
    /// manual evaluation by a teacher or human.
    /// </summary>
    Manual,

    /// <summary>
    /// constamt rule-based evaluation (e.g., true/false).
    /// </summary>
    RuleBased,

    /// <summary>
    /// ai based evaluation, where the AI system automatically assesses the answer.
    /// </summary>
    AI,

    /// <summary>
    /// peer review, where students evaluate each other's answers.
    /// </summary>
    PeerReview,

    /// <summary>
    /// hybrid evaluation, combining AI and human input (e.g., AI suggestion + teacher approval).
    /// </summary>
    Hybrid,

    /// <summary>
    /// Represents an external evaluation source, such as a third-party system.
    /// </summary>
    External
}
