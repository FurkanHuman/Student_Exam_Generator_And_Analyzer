using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Analysis : Entity<int>
{
    public string Name { get; set; }
    public string AIResponse { get; set; }
    public int SemesterId { get; set; }
    public int PrincipalId { get; set; }
    public int SchoolId { get; set; }
    public int LessonId { get; set; }
    public int ReferenceBenefitId { get; set; }

    public Semester Semester { get; set; }
    public Principal Principal { get; set; }
    public School School { get; set; }
    public Lesson Lesson { get; set; }
    public ReferenceBenefit ReferenceBenefit { get; set; }

    public IList<Exam> Exams { get; set; }
    public IList<StudentExamAnswer> StudentExamAnswers { get; set; }
    public IList<Teacher> Teachers { get; set; }
}
