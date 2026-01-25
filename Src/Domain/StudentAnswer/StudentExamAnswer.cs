using Domain.Common;
using Domain.Common.Enums;

namespace Domain.StudentAnswer;

public sealed class StudentExamAnswer : Entity<Guid>
{
    public int ReviewerTeacherId { get; private set; }
    public int StudentId { get; private set; }
    public int ExamId { get; private set; }

    public EvaluationOrigin EvaluationOrigin { get; private set; }
    public ExamEvaluationStatus EvaluationStatus { get; private set; }

    private readonly List<StudentAnswer> _answers = [];
    public IReadOnlyList<StudentAnswer> Answers => _answers;

    private StudentExamAnswer() { }

    public static StudentExamAnswer Create(int studentId, int examId, int reviewerTeacherId)
    {
        return new StudentExamAnswer
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            ExamId = examId,
            ReviewerTeacherId = reviewerTeacherId,
            EvaluationOrigin = EvaluationOrigin.NotEvaluated,
            EvaluationStatus = ExamEvaluationStatus.NotEvaluated
        };
    }

    public void AddAnswer(StudentAnswer answer)
    {
        if (_answers.Any(a => a.QuizQuestionId == answer.QuizQuestionId))
            throw new DomainException("Answer for this question already exists");
        _answers.Add(answer);
    }

    public void Evaluate(EvaluationOrigin origin)
    {
        if (_answers.Count == 0)
            throw new DomainException("Cannot evaluate exam with no answers");

        if (_answers.Any(a => !a.IsEvaluated()))
            throw new DomainException("All answers must be evaluated first");

        EvaluationOrigin = origin;
        EvaluationStatus = ExamEvaluationStatus.Evaluated;
    }

    public void MarkAsExcused(bool withReport = false)
    {
        EvaluationStatus = withReport
            ? ExamEvaluationStatus.ExcusedWithReport
            : ExamEvaluationStatus.Excused;
    }

    public void MarkAsInvalid() => EvaluationStatus = ExamEvaluationStatus.Invalid;

    public int GetTotalScore()
    {
        return _answers
            .Where(a => a.GivenScore != null)
            .Sum(a => a.GivenScore!.Value);
    }

    public int GetMaxScore()
    {
        return _answers
            .Where(a => a.GivenScore != null)
            .Sum(a => a.GivenScore!.MaxValue);
    }
}
