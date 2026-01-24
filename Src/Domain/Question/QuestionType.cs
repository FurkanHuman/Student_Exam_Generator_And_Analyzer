namespace Domain.Question;

public enum QuestionType : byte
{
    Undefined = 0,
    OpenEnded = 1,
    ClosedEnded = 2,
    MultipleChoice = 3,
    TrueFalse = 4,
    FillInTheBlank = 5,
    Matching = 6,
    Ordering = 7
}
