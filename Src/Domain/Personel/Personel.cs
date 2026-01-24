using Domain.Common;
using Domain.Shared.ValueObjects;

namespace Domain.Personel;

public sealed class Personel : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public PersonName Name { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public PersonelStatus Status { get; private set; }

    private Personel()
    {
        Name = PersonName.Create("Default", "Name");
    }

    public static Personel Create(Guid userId,
                                  string name,
                                  string surname,
                                  DateOnly birthDate)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User ID cannot be empty");

        int age = CalculateAge(birthDate);
        if (age < 18)
            throw new DomainException("Personel must be at least 18 years old");

        return new Personel
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = PersonName.Create(name, surname),
            BirthDate = birthDate,
            Status = PersonelStatus.Active
        };
    }

    public void UpdateName(string name, string surname)
    {
        if (Status == PersonelStatus.Terminated)
            throw new DomainException("Cannot update terminated personel");

        Name = PersonName.Create(name, surname);
    }

    public void ChangePersonelStatus(PersonelStatus newStatus)
    {
        if (Status == PersonelStatus.Terminated)
            throw new DomainException("Cannot change status of terminated personel");
        Status = newStatus;
    }

    public void Activate()
    {
        if (Status == PersonelStatus.Active)
            throw new DomainException("Already active");
        ChangePersonelStatus(PersonelStatus.Active);
    }

    public void Suspend()
    {
        if (Status == PersonelStatus.Terminated)
            throw new DomainException("Cannot suspend terminated personel");
        ChangePersonelStatus(PersonelStatus.Suspended);
    }

    private static int CalculateAge(DateOnly birthDate)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age;
    }

    public void Terminate() => ChangePersonelStatus(PersonelStatus.Terminated);
    public int GetAge() => CalculateAge(BirthDate);
    public bool IsActive() => Status == PersonelStatus.Active;

}
