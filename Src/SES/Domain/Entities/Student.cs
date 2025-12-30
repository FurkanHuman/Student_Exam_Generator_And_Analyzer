using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Student : Entity<int>
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public string SchoolNumber { get; set; }
    public char Gender { get; set; }
    public string? Description { get; set; }
    public bool IsGhostStudent { get; set; }
    public int SchoolId { get; set; }
    public int StudentClassId { get; set; }

    public School School { get; set; }
    public StudentClass StudentClass { get; set; }

    public IList<Exam> Exams { get; set; }
    public IList<Teacher> Teachers { get; set; }

}