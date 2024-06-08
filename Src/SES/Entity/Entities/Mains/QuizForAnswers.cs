using NArchitecture.Core.Persistence.Repositories;

namespace Entity.Entities.Mains;

public class QuizForAnswers : Entity<int> // todo: eski kaldır.
{
    public IList<StudentQuizAnswer> QuizAnswers { get; set; }

}
