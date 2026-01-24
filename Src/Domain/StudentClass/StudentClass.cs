using Domain.Common;

namespace Domain.StudentClass;

public sealed class StudentClass : Entity<int>
{
    public string? Name { get; private set; }
    public int ClassAge { get; private set; }
    public char ClassBranch { get; private set; }
    public string? Description { get; private set; }

    public int SchoolId { get; private set; }
    public int SemesterId { get; private set; }
    public int RefTeacherId { get; private set; }

    private StudentClass() { }

    public static StudentClass Create(int classAge,
                                      char classBranch,
                                      int schoolId,
                                      int semesterId,
                                      int refTeacherId,
                                      string? name = null)
    {
        if (classAge < 0 || classAge > 12)
            throw new DomainException("Class age must be between 0 and 12");
        if (!char.IsLetter(classBranch))
            throw new DomainException("Class branch must be a letter");

        return new StudentClass
        {
            ClassAge = classAge,
            ClassBranch = char.ToUpper(classBranch),
            SchoolId = schoolId,
            SemesterId = semesterId,
            RefTeacherId = refTeacherId,
            Name = name ?? $"{classAge}-{char.ToUpper(classBranch)}"
        };
    }

    public void SetDescription(string? description) => Description = description?.Trim();
}
