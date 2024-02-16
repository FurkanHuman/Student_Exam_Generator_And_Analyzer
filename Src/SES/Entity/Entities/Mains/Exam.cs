using Entity.Base;

namespace Entity.Entities.Mains;

public class Exam : Entity<int>
{
    public string ExamSemesterYear { get; set; } // note: sınav dönemi Yılı. örnek 2000 - 2001
    public string LessonName { get; set; } // note: sınav adı
    public string Semester { get; set; } // note: dönem . örnek 1. Dönem 2. Dönem...
    public string SemesterSesion { get; set; } // note: bir dönemdeki sınavlar. örnek 1. sınav 2. sınav...
    public string ExamCode { get; set; }
    public string FooterNote { get; set; }

    public int AnalysisHeaderId { get; set; }
    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public int ReferenceBenefitId { get; set; }

    public Teacher Teacher { get; set; }
    public Student Student { get; set; }
    public School School { get; set; }
    public ReferenceBenefit ReferenceBenefit { get; set; }
    public IList<QuizQuestion> QuizQuestions { get; set; }
    public virtual AnalysisHeader AnalysisHeader { get; set; }
}
