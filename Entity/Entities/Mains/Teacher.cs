using Entity.Entities.Bases;

namespace Entity.Entities.Mains;

internal class Teacher:Person
{

    public int ExamId { get; set; }
    public int StudentId { get; set; }

    public required IList<Exam> Exams { get; set; }
    public required IList<Student> Students { get; set; }
}
