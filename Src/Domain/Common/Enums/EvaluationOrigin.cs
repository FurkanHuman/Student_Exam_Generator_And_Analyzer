namespace Domain.Common.Enums;

public enum EvaluationOrigin : byte
{
    NotEvaluated = 0,
    Manual = 1,
    RuleBased = 2,
    AI = 3,
    PeerReview = 4,
    Hybrid = 5,
    External = 6
}
