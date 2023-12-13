using Entity.Entities.Bases;

namespace Entity.Entities.Mains;

    internal class Student:Person
{
    public required string Name { get; set; }
    public required string SurName { get; set; }
    public string? Class { get; set; }
    public string? School { get; set; }
    public required string SchoolNumber { get; set; }
    public string? Description { get; set; }

    public int TeacherId { get; set; }
    public int ExamId { get; set; }

    public required IList<Exam> Exams { get; set; }

    public required IList<Teacher> Teachers { get; set; }
}

