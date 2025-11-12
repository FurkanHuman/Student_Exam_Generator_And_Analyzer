using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class StudentAnswer : Entity<Guid>
{
    public Guid StudentExamAnswerId { get; set; }
    public virtual StudentExamAnswer StudentExamAnswer { get; set; }

    public int QuizQuestionId { get; set; }
    public virtual QuizQuestion QuizQuestion { get; set; }

    public Guid? QuestionOptionId { get; set; }
    public virtual QuestionOption? QuestionOption { get; set; }

    public string? AnswerText { get; set; }
    public int? GivenScore { get; set; }

    public EvaluationStatus EvaluationStatus { get; set; }

}