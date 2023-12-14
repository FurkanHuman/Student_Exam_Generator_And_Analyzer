using Entity.Base;

namespace Entity.Entities.Mains;

public class Exam : Entity<int>
{
    public required string ExamSemesterYear { get; set; } // note: sınav dönemi Yılı. örnek 2000 - 2001
    public required string LessonName { get; set; } // note: sınav adı
    public required string Semester { get; set; } // note: dönem . örnek 1. Dönem 2. Dönem...
    public required string SemesterSesion { get; set; } // note: bir dönemdeki sınavlar. örnek 1. sınav 2. sınav...
    public required string FooterNote { get; set; }
    public int QuizQuestionId { get; set; }
    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public required string ExamCode { get; set; }

    public required Teacher Teacher { get; set; }
    public required Student Student { get; set; }
    public required School School { get; set; }
    public required IList<QuizQuestion> QuizQuestions { get; set; }

}
