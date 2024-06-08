using Domain.Entities;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Students.Commands.Create;

public class CreatedStudentResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int ClassAge { get; set; }
    public char ClassBranch { get; set; }
    public string SchoolNumber { get; set; }
    public char Gender { get; set; }
    public string? Description { get; set; }
    public int SchoolId { get; set; }
    public int TeacherId { get; set; }
    public int ExamId { get; set; }
    public int SemesterId { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
}