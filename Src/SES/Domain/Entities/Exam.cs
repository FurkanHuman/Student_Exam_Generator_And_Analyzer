using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Exam : Entity<int>
{
    public DateOnly ExamDate { get; set; }
    public string ExamLessonName { get; set; }
    public string ExamTrackingCode { get; set; }
    public byte[] ExamRandomizerSeed { get; set; }
    public string FooterNote { get; set; }
    public string? TotalScoreForString { get; set; }
    public int? TotalScore { get; set; }

    public int ExamConfigurationId { get; set; }
    public int LessonId { get; set; }
    public int SemesterId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public int ReferenceBenefitId { get; set; }
    public int ExamAuthorId { get; set; }

    public ExamConfiguration ExamConfiguration { get; set; }
    public Lesson Lesson { get; set; }
    public Semester Semester { get; set; }
    public Student Student { get; set; }
    public School School { get; set; }
    public ReferenceBenefit ReferenceBenefit { get; set; }
    public Teacher ExamAuthor { get; set; }
    public StudentExamAnswer StudentExamAnswer { get; set; }

    public EvaluationOrigin EvaluationOrigin { get; set; }
    public Dictionary<int, int> QuestionOrderMap { get; set; } // key is question Id, value is exam question order

    public IList<Analysis> Analyses { get; set; }
    public IList<Teacher> Teachers { get; set; }
    public IList<StudentClass> StudentClasses { get; set; }
    public IList<QuizQuestion> QuizQuestions { get; set; }
}
