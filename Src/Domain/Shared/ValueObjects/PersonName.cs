using Domain.Common;

namespace Domain.Shared.ValueObjects;

public sealed record PersonName : ValueObject
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public string FullName => $"{Name} {Surname}";

    private PersonName()
    {
        Name = string.Empty;
        Surname = string.Empty;
    }

    private PersonName(string name, string surname)
    {
        ThrowIfNameOrSurnameEmpty(name, surname);
        ThrowIfNameOrSurnameOutOfRange(name, surname);

        Name = NormalizeName(name);
        Surname = NormalizeName(surname);
    }

    private static void ThrowIfNameOrSurnameOutOfRange(string name, string surname)
    {
        if (name.Length < 2 || name.Length > 64)
            throw new DomainException("Name must be between 2 and 64 characters");

        if (surname.Length < 2 || surname.Length > 64)
            throw new DomainException("Surname must be between 2 and 64 characters");
    }

    private static void ThrowIfNameOrSurnameEmpty(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(surname))
            throw new DomainException("Surname cannot be empty");
    }

    public static PersonName Create(string name, string surname) => new(name, surname);

    private static string NormalizeName(string name)
    {
        name = name.Trim();
        return char.ToUpper(name[0]) + name[1..].ToLower();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Surname;
    }
}
