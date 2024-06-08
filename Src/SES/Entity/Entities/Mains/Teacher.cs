using Entity.Entities.Bases;
using Entity.Entities.Infos;

namespace Entity.Entities.Mains;

public class Teacher : Person
{
    public int SemesterId { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public School School { get; set; }

    public Semester Semester { get; set; }
    public IList<ReferenceBenefit> ReferenceBenefits { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Student> Students { get; set; }

}
