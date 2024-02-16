using Entity.Base;

namespace Entity.Entities.Mains;

public class Benefit : Entity<int>
{
    public int SubLearningAreaId { get; set; }

    public string ReferenceBenefitNumber { get; set; }
    public string ReferenceBenefitComments { get; set; }

    public virtual SubLearningArea SubLearningArea { get; set; }

    public virtual IList<QuizQuestion> QuizQuestions { get; set; }
}