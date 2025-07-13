using NArchitecture.Core.Persistence.Repositories;
using Domain.Enums;

namespace Domain.Entities;

public class StudentAnswer : Entity<Guid>
{
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    public int ExamId { get; set; }
    public virtual Exam Exam { get; set; }

    public int QuizQuestionId { get; set; }
    public virtual QuizQuestion QuizQuestion { get; set; }

    public Guid? QuestionOptionId { get; set; }
    public virtual QuestionOption? QuestionOption { get; set; }

    public string? AnswerText { get; set; }
    public int? GivenScore { get; set; }

    public EvaluationOrigin EvaluationOrigin { get; set; }
    public EvaluationStatus EvaluationStatus { get; set; }
}