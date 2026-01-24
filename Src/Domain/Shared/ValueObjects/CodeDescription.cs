using Domain.Common;

namespace Domain.Shared.ValueObjects;

public sealed record CodeDescription : ValueObject
{
    public string Code { get; init; }
    public string Description { get; init; }

    private CodeDescription()
    {
        Code = string.Empty;
        Description = string.Empty;
    }

    private CodeDescription(string code, string description)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Code cannot be empty");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description cannot be empty");

        Code = code.Trim().ToUpper();
        Description = description.Trim();
    }

    public static CodeDescription Create(string code, string description) => new(code, description);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
        yield return Description;
    }
}
