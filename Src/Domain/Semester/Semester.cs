using Domain.Common;
using Domain.Shared.ValueObjects;

namespace Domain.Semester;

public sealed class Semester : Entity<int>
{
    public string Name { get; private set; }
    public DateRange Period { get; private set; }

    private Semester()
    {
        Name = string.Empty;
        Period = DateRange.Create(DateOnly.MinValue, DateOnly.MaxValue);
    }

    public static Semester Create(string name, DateOnly startDate, DateOnly endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Semester name cannot be empty");

        return new Semester
        {
            Name = name.Trim(),
            Period = DateRange.Create(startDate, endDate)
        };
    }

    public void UpdatePeriod(DateOnly startDate, DateOnly endDate) => Period = DateRange.Create(startDate, endDate);

    public bool IsActive() => Period.IsActive();
}
