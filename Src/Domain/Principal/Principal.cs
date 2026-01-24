using Domain.Common;

namespace Domain.Principal;

public sealed class Principal : Entity<int>
{
    public Guid PersonelId { get; private set; }
    public int SemesterId { get; private set; }

    private Principal() { }

    public static Principal Create(Guid personelId, int semesterId)
    {
        if (personelId == Guid.Empty)
            throw new DomainException("Personel ID cannot be empty");

        return new Principal
        {
            PersonelId = personelId,
            SemesterId = semesterId
        };
    }
}
