using NArchitecture.Core.Persistence.Repositories;

namespace Entity.Entities.Mains;

public class LearningArea : Entity<int>
{
    public required string Name { get; set; }

    public int SubLearningAreaId { get; set; }

    public IList<SubLearningArea> SubLearningAreas { get; set; }
}
