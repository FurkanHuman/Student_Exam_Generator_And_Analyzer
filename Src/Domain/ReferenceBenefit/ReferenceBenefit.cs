using Domain.Common;

namespace Domain.ReferenceBenefit;

public sealed class ReferenceBenefit : Entity<int>
{
    public string Name { get; private set; }
    public int LessonId { get; private set; }
    public int SchoolId { get; private set; }
    public int SemesterId { get; private set; }

    private ReferenceBenefit()
    {
        Name = string.Empty;
    }

    public static ReferenceBenefit Create(string name,
                                          int lessonId,
                                          int schoolId,
                                          int semesterId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Reference benefit name cannot be empty");

        return new ReferenceBenefit
        {
            Name = name.Trim(),
            LessonId = lessonId,
            SchoolId = schoolId,
            SemesterId = semesterId
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty");
        Name = name.Trim();
    }
}
