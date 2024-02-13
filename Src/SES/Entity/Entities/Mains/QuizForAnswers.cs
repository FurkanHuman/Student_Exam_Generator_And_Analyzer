using Entity.Base;

namespace Entity.Entities.Mains;

public class QuizForAnswers : Entity<int>
{
    public IList<StudentQuizAnswer> QuizAnswers { get; set; }

}
