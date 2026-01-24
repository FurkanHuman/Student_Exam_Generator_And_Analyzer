using Domain.Common;
using Domain.Common.Enums;
using Domain.Exam.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.Exam;

public sealed class Exam : Entity<int>
{
    public ExamMetadata Metadata { get; private set; }
    public DateOnly ExamDate { get; private set; }
    public Score? TotalScore { get; private set; }
    public int ExamConfigurationId { get; private set; }
    public int LessonId { get; private set; }
    public int SemesterId { get; private set; }
    public int StudentId { get; private set; }
    public int SchoolId { get; private set; }
    public int ReferenceBenefitId { get; private set; }
    public int ExamAuthorId { get; private set; }
    public EvaluationOrigin EvaluationOrigin { get; private set; }

    private readonly Dictionary<int, int> _questionOrderMap = [];
    public IReadOnlyDictionary<int, int> QuestionOrderMap => _questionOrderMap;

    private Exam()
    {
        Metadata = ExamMetadata.Create("Default", "000");
    }

    public static Exam CreateBase(string lessonName,
                                  string trackingCode,
                                  DateOnly examDate)
    {
        return new Exam
        {
            Metadata = ExamMetadata.Create(lessonName, trackingCode),
            ExamDate = examDate,
            ReferenceBenefitId = 0,
            EvaluationOrigin = EvaluationOrigin.NotEvaluated
        };
    }

    public Exam WithContext(int configurationId,
                            int lessonId,
                            int semesterId,
                            int studentId,
                            int schoolId,
                            int authorId)
    {
        ExamConfigurationId = configurationId;
        LessonId = lessonId;
        SemesterId = semesterId;
        StudentId = studentId;
        SchoolId = schoolId;
        ExamAuthorId = authorId;
        return this;
    }

    public void SetQuestionOrder(int questionId, int order) => _questionOrderMap[questionId] = order;

    public void SetTotalScore(int score, int maxScore) => TotalScore = Score.Create(score, maxScore);

    public void MarkAsEvaluated(EvaluationOrigin origin)
    {
        if (EvaluationOrigin != EvaluationOrigin.NotEvaluated)
            throw new DomainException("Exam already evaluated");
        EvaluationOrigin = origin;
    }
}
