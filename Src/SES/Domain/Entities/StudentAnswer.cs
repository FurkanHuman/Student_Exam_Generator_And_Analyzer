using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class StudentAnswer : Entity<Guid> // öğrencilerin sorulara verdiği cevaplar
{
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    public int QuizQuestionId { get; set; }
    public virtual QuizQuestion QuizQuestion { get; set; }

    public Guid? QuestionOptionId { get; set; }
    public virtual QuestionOption? QuestionOption { get; set; }

    public int QuestionScoreId { get; set; }
    public QuestionScore QuestionScore { get; set; }

    public string? AnswerText { get; set; }
    public bool IsCorrect { get; set; }
}
