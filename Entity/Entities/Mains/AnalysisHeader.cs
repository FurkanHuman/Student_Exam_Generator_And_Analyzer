using Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities.Mains;

public class AnalysisHeader : Entity<int>
{
    public int ClassAge { get; set; } // sınıf yaşı 
    public required char AltClass { get; set; } // sıjnıf şubesi tek hane
    public string ExamSemesterYear { get; set; } // note: sınav dönemi Yılı. örnek 2000 - 2001
    public string LessonName { get; set; } // note: sınav adı
    public string Semester { get; set; } // note: dönem . örnek 1. Dönem 2. Dönem...
    public string SemesterSession { get; set; } // note: bir dönemdeki sınavlar. örnek 1. sınav 2. sınav...
    public int[] RefScorePerQuestions { get; set; }
    public string[] RefBeneFitsNo { get; set; }
    public string[] RefBeneFitCommets { get; set; }

    public string ExamCode { get; set; }
    public string FooterNote { get; set; }

    public int TeacherId { get; set; }
    public int PrincipalId { get; set; }
    public int SqaId { get; set; }
    public int SchoolId { get; set; }


    public Teacher Teacher { get; set; }
    public Principal Principal { get; set; }
    public School School { get; set; }
    public IList<StudentQuizAnswer> StudentQuizAnswers { get; set; }
}
