using Domain.Common;

namespace Domain.Teacher;

public sealed class Teacher : Entity<int>
{
    public Guid PersonelId { get; private set; }
    public int SchoolId { get; private set; }
    public int SemesterId { get; private set; }

    private Teacher() { }

    public static Teacher Create(Guid personelId, int schoolId, int semesterId)
    {
        if (personelId == Guid.Empty)
            throw new DomainException("Personel ID cannot be empty");

        return new Teacher
        {
            PersonelId = personelId,
            SchoolId = schoolId,
            SemesterId = semesterId
        };
    }
}
