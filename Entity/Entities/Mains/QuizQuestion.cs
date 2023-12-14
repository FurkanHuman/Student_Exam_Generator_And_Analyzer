using Entity.Base;

namespace Entity.Entities.Mains;

public class QuizQuestion:Entity<int>
{
    public int ExamId { get; set; }
    public required string Question { get; set; }
    public string? QuestionImage { get; set; }
    public required string QuestionLesson { get; set; }
    public required int QuestionLessonUnit { get; set; } // note: dersin ünitesi
    public int LuckyFactor { get; set; }

    public required IList<Exam> Exam { get; set; }
}
