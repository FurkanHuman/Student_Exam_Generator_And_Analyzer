using Entity.Base;

namespace Entity.Entities.Mains;

public class QuestionScore : Entity<int>
{
    public int Score { get; set; }
    public int MaxScore { get; set; }

    public virtual ReferenceBenefit ReferenceBenefit { get; set; }
}
