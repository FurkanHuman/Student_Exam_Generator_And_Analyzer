using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;
public class QuizQuestion : Entity<int> // note: sorununn kendisi.
{

    public int QuestionScoreId { get; set; }
    public string Question { get; set; }
    public string QuestionBody { get; set; }
    public string QuestionImageURL { get; set; }
    public QuestionType QuestionType { get; set; }
    public QuestionScore QuestionScore { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Benefit> Benefits { get; set; }
    public IList<QuestionOption> Options { get; set; }
    public IList<StudentAnswer> StudentAnswers { get; set; }
}
