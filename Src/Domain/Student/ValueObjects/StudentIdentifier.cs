using Domain.Common;

namespace Domain.Student.ValueObjects;

public sealed record StudentIdentifier : ValueObject
{
    public string SchoolNumber { get; init; }

    private StudentIdentifier()
    {
        SchoolNumber = string.Empty;
    }

    private StudentIdentifier(string schoolNumber)
    {
        if (string.IsNullOrWhiteSpace(schoolNumber))
            throw new DomainException("School number cannot be empty");

        if (schoolNumber.Length < 2 || schoolNumber.Length > 20)
            throw new DomainException("School number must be between 2 and 20 characters");

        SchoolNumber = schoolNumber.Trim();
    }

    public static StudentIdentifier Create(string schoolNumber) => new(schoolNumber);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return SchoolNumber;
    }
}
