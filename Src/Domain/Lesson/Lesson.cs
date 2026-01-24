using Domain.Common;
using Domain.Shared.ValueObjects;

namespace Domain.Lesson;

public sealed class Lesson : Entity<int>
{
    public string LessonName { get; private set; }
    public string Description { get; private set; }
    public Score PassingCriteria { get; private set; }
    public bool IsMandatory { get; private set; }
    public int ClassAge { get; private set; }
    public int SemesterId { get; private set; }

    private Lesson()
    {
        LessonName = string.Empty;
        Description = string.Empty;
        PassingCriteria = Score.Zero(100);
    }

    public static Lesson Create(string name,
                                string description,
                                int passingScore,
                                int maxScore,
                                int classAge,
                                int semesterId,
                                bool isMandatory = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Lesson name cannot be empty");
        if (classAge < 0 || classAge > 12)
            throw new DomainException("Class age must be between 0 and 12");

        return new Lesson
        {
            LessonName = name.Trim(),
            Description = description.Trim(),
            PassingCriteria = Score.Create(passingScore, maxScore),
            ClassAge = classAge,
            SemesterId = semesterId,
            IsMandatory = isMandatory
        };
    }

    public void UpdatePassingScore(int passingScore, int maxScore) => PassingCriteria = Score.Create(passingScore, maxScore);
    public void MarkAsMandatory() => IsMandatory = true;
    public void MarkAsElective() => IsMandatory = false;
}
