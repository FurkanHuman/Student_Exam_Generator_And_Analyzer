using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;
public class Student : Entity<int>
{
    public string Name { get; set; }
    public string SurName { get; set; }
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