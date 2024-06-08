using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;
public class StudentClass:Entity<int>
{
    public string? Name { get; set; }
    public int ClassAge { get; set; }
    public required char ClassBranch { get; set; }
    public string? Decription { get; set; }

    public int SchoolId { get; set; }
    public int SemesterId { get; set; }
    public int RefTeacherId { get; set; }

    public School School { get; set; }
    public Semester Semester { get; set; }
    public Teacher RefTeacher { get; set; }

    public IList<Student> Students { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Analysis> Analyses { get; set; }
    public IList<Teacher> Teachers { get; set; }
}
