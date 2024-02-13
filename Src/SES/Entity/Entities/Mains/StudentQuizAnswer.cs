using Entity.Base;

namespace Entity.Entities.Mains;

public class StudentQuizAnswer : Entity<int>
{
    public string StudentNumber { get; set; }
    public int ExamId { get; set; }
    public QuestionScore[] ScorePerQuestions { get; set; }
    public int TotalScore { get; set; }
    public char[] SpecialStatuses { get; set; }
    public Student Student { get; set; }
    public Exam Exam { get; set; }
}
