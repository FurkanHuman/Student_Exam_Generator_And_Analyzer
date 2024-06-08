using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Entity.Entities.Infos;

public class Semester : Entity<int>
{
    public string Name { get; set; }
    public DateOnly BeginSemesterDate { get; set; }

    public DateOnly EndSemesterDate { get; set; }

    public IList<Analysis> Analyses { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Principal> Principals { get; set; }
    public IList<ReferenceBenefit> ReferenceBenefits { get; set; }
    public IList<Teacher> Teachers { get; set; }
    public IList<Student> Students { get; set; }
}
