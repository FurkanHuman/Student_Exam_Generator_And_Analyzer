using Entity.Entities.Bases;

namespace Entity.Entities.Mains;
public class Student : Person
{
    public int ClassAge { get; set; } // sınıf yaşı 
    public required char ClassBranch{ get; set; } // sıjnıf şubesi tek hane
    public string SchoolNumber { get; set; }
    public string? Description { get; set; }
    public int SchoolId { get; set; }
    public int TeacherId { get; set; }
    public int ExamId { get; set; }

    public School School { get; set; }

    public IList<Exam> Exams { get; set; }

    public IList<Teacher> Teachers { get; set; }
}