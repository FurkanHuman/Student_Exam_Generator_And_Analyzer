using Domain.Common;

namespace Domain.Shared.ValueObjects;

public sealed record Gender : ValueObject
{
    public char Value { get; }

    private Gender(char value)
    {
        Value = value;
    }

    public static readonly Gender Female = new('F');
    public static readonly Gender Male = new('M');
    public static readonly Gender Other = new('O');
    public static readonly Gender Unspecified = new('U');

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
