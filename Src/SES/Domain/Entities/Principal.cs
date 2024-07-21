using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Principal : Entity<int>
{
    public Guid PersonelId { get; set; }
    public int SemesterId { get; set; }

    public virtual Personel Personel { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }

    public IList<Analysis> Analyses { get; set; }
}
