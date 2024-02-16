using Entity.Base;

namespace Entity.Entities.Mains;

public class SubLearningArea : Entity<int>
{
    public required string Name { get; set; }

    public int BenefitId { get; set; }

    public virtual IList<Benefit> Benefits { get; set; }
}
