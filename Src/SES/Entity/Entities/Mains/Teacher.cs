using Entity.Entities.Bases;

namespace Entity.Entities.Mains;

public class Teacher : Person
{
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }

    public IList<ReferenceBenefit> ReferenceBenefits { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Student> Students { get; set; }
    public IList<School> Schools { get; set; }

}
