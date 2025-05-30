namespace Domain.Enums;

/// <summary>
/// Represents the various types of questions.
/// </summary>
public enum QuestionType : byte
{
    /// <summary>
    /// The question type is not defined.
    /// </summary>
    Undefined,

    /// <summary>
    /// An open-ended question requiring a detailed answer.
    /// </summary>
    OpenEnded,

    /// <summary>
    /// A closed-ended question with predefined answer options.
    /// </summary>
    ClosedEnded,

    /// <summary>
    /// A multiple-choice question where one or more options can be selected.
    /// </summary>
    MultipleChoice,

    /// <summary>
    /// A question with a binary true or false answer.
    /// </summary>
    TrueFalse,

    /// <summary>
    /// A question where users complete missing parts of a statement.
    /// </summary>
    FillInTheBlank,

    /// <summary>
    /// A question that requires matching elements from two sets.
    /// </summary>
    Matching,

    /// <summary>
    /// A question that involves ordering items correctly.
    /// </summary>
    Ordering
}
