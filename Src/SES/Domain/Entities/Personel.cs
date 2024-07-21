using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;
public class Personel : Entity<Guid>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public DateOnly BirthDate { get; set; }
    public virtual User User { get; set; }
}
