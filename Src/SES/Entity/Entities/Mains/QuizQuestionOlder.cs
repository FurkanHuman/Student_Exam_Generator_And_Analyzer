using Entity.Base;

namespace Entity.Entities.Mains;

public class QuizQuestionOlder : Entity<int>
{
    public int ExamId { get; set; }
    public string Question { get; set; }
    public string QuestionImage { get; set; }
    public int LuckyFactor { get; set; }
}
