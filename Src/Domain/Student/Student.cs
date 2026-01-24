using Domain.Common;
using Domain.Shared.ValueObjects;
using Domain.Student.ValueObjects;

namespace Domain.Student;

public sealed class Student : Entity<int>
{
    public PersonName Name { get; private set; }
    public StudentIdentifier Identifier { get; private set; }
    public Gender Gender { get; private set; }
    public string? Description { get; private set; }
    public bool IsGhostStudent { get; private set; }

    public int SchoolId { get; private set; }
    public int StudentClassId { get; private set; }
    public int? PreviousStudentId { get; private set; }

    private Student()
    {
        Name = PersonName.Create("Default", "Name");
        Identifier = StudentIdentifier.Create("000");
        Gender = Gender.Unspecified;
    }

    public static Student Create(string name,
                                 string surname,
                                 string schoolNumber,
                                 Gender gender,
                                 int schoolId,
                                 int studentClassId,
                                 bool isGhost = false)
    {
        return new Student
        {
            Name = PersonName.Create(name, surname),
            Identifier = StudentIdentifier.Create(schoolNumber),
            Gender = gender,
            IsGhostStudent = isGhost,
            SchoolId = schoolId,
            StudentClassId = studentClassId
        };
    }

    public void UpdateName(string name, string surname) => Name = PersonName.Create(name, surname);

    public void SetDescription(string? description) => Description = description?.Trim();

    public void MarkAsGhost() => IsGhostStudent = true;

    public void LinkToPreviousStudent(int previousStudentId)
    {
        if (previousStudentId == Id)
            throw new DomainException("Cannot link student to itself");
        PreviousStudentId = previousStudentId;
    }
}
