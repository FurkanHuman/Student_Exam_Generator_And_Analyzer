using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Lesson : Entity<int>
{
    public string LessonName { get; set; }
    public string Description { get; set; }
    public int SemesterId { get; set; }

    public Semester Semester { get; set; }
    public IList<StudentClass> StudentClasses { get; set; }
    public IList<ReferenceBenefit> ReferenceBenefits { get; set; }
    public IList<Teacher> Teachers { get; set; }
}