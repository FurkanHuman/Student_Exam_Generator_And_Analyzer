using Entity.Base;

namespace Entity.Entities.Mains;

public class LearningArea : Entity<int>
{
    public required string Name { get; set; }

    public int SubLearningAreaId { get; set; }

    public IList<SubLearningArea> SubLearningAreas { get; set; }
}
