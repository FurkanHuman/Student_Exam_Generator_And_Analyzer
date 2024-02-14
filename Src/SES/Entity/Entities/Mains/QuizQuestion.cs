using Entity.Base;

namespace Entity.Entities.Mains;

public class QuizQuestionOlder : Entity<int>
{
    public int ExamId { get; set; }
    public string Question { get; set; }
    public string QuestionImage { get; set; }
    public int LuckyFactor { get; set; }
}
public class QuizQuestion : Entity<int>
{
    public int BenefitId { get; set; }
    public int ExamId { get; set; }
    public string Question { get; set; }
    public string QuestionImage { get; set; }

    public virtual IList<Exam> Exams { get; set; }
    public virtual IList<Benefit> Benefits { get; set; }
}