using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Teacher : Entity<int>
{
    public Guid PersonelId { get; set; }
    public int SchoolId { get; set; }
    public int SemesterId { get; set; }

    public Personel Personel { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
    public IList<ReferenceBenefit> ReferenceBenefits { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Exam> ExamAuthors { get; set; }
    public IList<Student> Students { get; set; }
    public IList<Lesson> Lessons { get; set; }
}
