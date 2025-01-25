using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class SubLearningArea : Entity<int>
{
    public required string SLACode { get; set; }
    public required string Description { get; set; }

    public virtual IList<Benefit> Benefits { get; set; }
}
