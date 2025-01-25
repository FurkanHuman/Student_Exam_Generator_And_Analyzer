using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class ReferenceBenefit : Entity<int>
{
    public string ReferenceBenefitName { get; set; }
    public int LessonId { get; set; }
    public int SchoolId { get; set; }
    public int SemesterId { get; set; }

    public Lesson Lesson { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<QuizQuestion> QuizQuestions { get; set; }
    public IList<LearningArea> LearningAreas { get; set; }
}
