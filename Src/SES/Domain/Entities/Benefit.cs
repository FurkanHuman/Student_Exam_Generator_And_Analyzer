using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Benefit : Entity<int>
{
    public required string BenefitCode { get; set; }
    public required string Description { get; set; }

    public virtual SubLearningArea SubLearningArea { get; set; }

    public virtual IList<QuizQuestion> QuizQuestions { get; set; }
}