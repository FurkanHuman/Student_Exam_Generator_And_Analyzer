using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class StudentExamAnswer: Entity<Guid>
{
    public int ReviewerTeacherId { get; set; }
    public virtual Teacher ReviewerTeacher { get; set; }

    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    public int ExamId { get; set; }
    public virtual Exam Exam { get; set; }

    public IList<StudentAnswer> StudentAnswers { get; set; }
    public EvaluationOrigin EvaluationOrigin { get; set; }
    public ExamEvaluationStatus ExamEvaluationStatus { get; set; }
}
