namespace Domain.Common.ValueObjects;

public sealed record EntityHash : ValueObject
{
    public string Value { get; init; }
    public DateTime ComputedAt { get; init; }

    private EntityHash()
    {
        Value = string.Empty;
        ComputedAt = DateTime.MinValue;
    }

    private EntityHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Hash cannot be empty");

        Value = value;
        ComputedAt = DateTime.UtcNow;
    }

    public static EntityHash Compute(string serializedData)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(serializedData);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
        string hash = Convert.ToBase64String(hashBytes);
        return new EntityHash(hash);
    }

    public bool Verify(string serializedData)
    {
        EntityHash newHash = Compute(serializedData);
        return Value == newHash.Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
