using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class SubLearningArea : Entity<int>
{
    public required string Name { get; set; }
    public virtual IList<Benefit> Benefits { get; set; }
}
