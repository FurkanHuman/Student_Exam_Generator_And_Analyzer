using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Teacher : Entity<int>
{

    public string Name { get; set; }
    public string SurName { get; set; }
    public int SemesterId { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
    public IList<ReferenceBenefit> ReferenceBenefits { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Student> Students { get; set; }

}
