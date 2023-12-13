using Entity.Base;

namespace Entity.Entities.Mains;

internal class Exam : Entity<int>
{
    public required string LessonName { get; set; }
    public int QuizQuestionId { get; set; }

    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    public required string ExamCode { get; set; }
    public required Teacher Teacher { get; set; }
    public required Student Student { get; set; }
    public required IList<QuizQuestion> QuizQuestions { get; set; }

}
