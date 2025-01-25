using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class LearningArea : Entity<int>
{
    public required string LACode { get; set; }
    public required string Description { get; set; }
    public IList<SubLearningArea> SubLearningAreas { get; set; }
}
