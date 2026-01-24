using Domain.Common;

namespace Domain.Shared.ValueObjects;

public sealed record Score : ValueObject
{
    public int Value { get; init; }
    public int MaxValue { get; init; }
    public decimal Percentage => MaxValue > 0 ? (decimal)Value / MaxValue * 100 : 0;

    private Score()
    {
        Value = 0;
        MaxValue = 0;
    }

    private Score(int value, int maxValue)
    {
        if (maxValue <= 0)
            throw new DomainException("Max score must be positive");
        if (value < 0)
            throw new DomainException("Score cannot be negative");
        if (value > maxValue)
            throw new DomainException("Score cannot exceed max score");

        Value = value;
        MaxValue = maxValue;
    }

    public static Score Create(int value, int maxValue) => new(value, maxValue);
    public static Score Zero(int maxValue) => new(0, maxValue);
    public bool IsPassing(int passingScore) => Value >= passingScore;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return MaxValue;
    }
}
