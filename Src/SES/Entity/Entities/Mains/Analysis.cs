using Entity.Entities.Infos;
using NArchitecture.Core.Persistence.Repositories;

namespace Entity.Entities.Mains;

public class Analysis : Entity<int>// todo: büyük değüişiklikler yolda
{
    public int ClassAge { get; set; } // sınıf yaşı 
    public required char AltClass { get; set; } // sınıf şubesi tek hane
    public string ExamSemesterYear { get; set; } // note: sınav dönemi Yılı. örnek 2000 - 2001
    public string LessonName { get; set; } // note: sınav adım
    public string LessonSession { get; set; } // note: bir dönemdeki sınavlar. örnek 1. sınav 2. sınav...

    public string ExamCode { get; set; }
    public string FooterNote { get; set; }

    public int SemesterId { get; set; }
    public int BenefitId { get; set; }
    public int QuestionId { get; set; }
    public int TeacherId { get; set; }
    public int PrincipalId { get; set; }
    public int StudentQuizAnswerId { get; set; }
    public int SchoolId { get; set; }

    public Semester Semester { get; set; }
    public Teacher Teacher { get; set; }
    public Principal Principal { get; set; }
    public School School { get; set; }
    public int[] RefScorePerQuestions { get; set; }
    public IList<StudentAnswer> Questions { get; set; }
    public IList<Benefit> Benefits { get; set; }
    public IList<StudentQuizAnswer> StudentQuizAnswers { get; set; } // Todo : kaldır 
}
