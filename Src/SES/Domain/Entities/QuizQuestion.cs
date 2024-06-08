using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;
public class QuizQuestion : Entity<int> // sorununn kendisi.
{
    public int BenefitId { get; set; }
    public int ExamId { get; set; }
    public int Score { get; set; }
    public string Question { get; set; }
    public string QuestionBody { get; set; }
    public string QuestionImage { get; set; }
    public QuestionType QuestionType { get; set; }
    public virtual IList<Exam> Exams { get; set; }
    public virtual IList<Benefit> Benefits { get; set; }
    public virtual IList<QuestionOption> Options { get; set; }
    public virtual IList<StudentAnswer> StudentAnswers { get; set; }
}
