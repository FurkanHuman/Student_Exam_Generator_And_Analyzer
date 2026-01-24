using Domain.Common;

namespace Domain.Shared.ValueObjects;

public sealed record DateRange : ValueObject
{
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int DurationInDays => EndDate.DayNumber - StartDate.DayNumber;

    private DateRange()
    {
        StartDate = DateOnly.MinValue;
        EndDate = DateOnly.MinValue;
    }

    private DateRange(DateOnly startDate, DateOnly endDate)
    {
        if (endDate <= startDate)
            throw new DomainException("End date must be after start date");

        StartDate = startDate;
        EndDate = endDate;
    }

    public static DateRange Create(DateOnly startDate, DateOnly endDate)
        => new(startDate, endDate);

    public bool Contains(DateOnly date) => date >= StartDate && date <= EndDate;
    public bool IsActive() => Contains(DateOnly.FromDateTime(DateTime.Today));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
