using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class QuestionScore : Entity<int>
{
    public int Score { get; set; }
    public int MaxScore { get; set; }

    public IList<QuizQuestion> QuizQuestions { get; set; }
    public IList<StudentAnswer> StudentAnswers { get; set; }
}
