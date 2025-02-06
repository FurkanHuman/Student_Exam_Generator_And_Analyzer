using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;
public class QuizQuestion : Entity<int> // note: sorununn kendisi.
{
    public string Question { get; set; } // soru girişi örnek: uzun hikaye yada sıralama sorusu girşi buraya yapılacak. ince fontlo sorular.
    public string QuestionBody { get; set; } // ana soru.
    public string QuestionImageURL { get; set; }
    public QuestionType QuestionType { get; set; }
    public QuestionScore QuestionScore { get; set; }
    public IList<Exam> Exams { get; set; }
    public IList<Benefit> Benefits { get; set; }
    public IList<QuestionOption> Options { get; set; }
    public IList<StudentAnswer> StudentAnswers { get; set; }
    public IList<Lesson> Lessons { get; set; }
}
