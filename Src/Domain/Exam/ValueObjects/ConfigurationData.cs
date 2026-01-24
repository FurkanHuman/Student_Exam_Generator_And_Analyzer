using Domain.Common;

namespace Domain.Exam.ValueObjects;

public sealed record ConfigurationData : ValueObject
{
    public string JsonString { get; init; }
    public string Hash { get; init; }

    private ConfigurationData()
    {
        JsonString = string.Empty;
        Hash = string.Empty;
    }

    private ConfigurationData(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new DomainException("Configuration cannot be empty");

        JsonString = jsonString;
        Hash = ComputeHash(jsonString);
    }

    public static ConfigurationData Create(string jsonString) => new(jsonString);

    private static string ComputeHash(string input)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Hash;
    }
}
