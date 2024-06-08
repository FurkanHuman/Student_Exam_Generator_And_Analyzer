using Entity.Entities.Bases;
using Entity.Entities.Infos;

namespace Entity.Entities.Mains;
public class Student : Person
{
    public int ClassAge { get; set; } // sınıf yaşı 
    public required char ClassBranch { get; set; } // sınıf şubesi tek hane
    public string SchoolNumber { get; set; }
    public char Gender { get; set; }
    public string? Description { get; set; }
    public int SchoolId { get; set; }
    public int TeacherId { get; set; }
    public int ExamId { get; set; }
    public int SemesterId { get; set; }

    public School School { get; set; }
    public Semester Semester { get; set; }

    public IList<Exam> Exams { get; set; }

    public IList<Teacher> Teachers { get; set; }

}