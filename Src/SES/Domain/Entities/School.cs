using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class School : Entity<int>
{
    public required string Name { get; set; }

    //public Principal Principal { get; set; }
    //public IList<Teacher> Teachers { get; set; }
    //public IList<Student> Students { get; set; }
    //public IList<Exam> Exams { get; set; }
    //public IList<Analysis> Analysis { get; set; }
    //public IList<ReferenceBenefit> ReferenceBenefits { get; set; }
}

