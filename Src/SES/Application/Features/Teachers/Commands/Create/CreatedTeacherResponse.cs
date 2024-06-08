using Domain.Entities;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Teachers.Commands.Create;

public class CreatedTeacherResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int SemesterId { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int SchoolId { get; set; }
    public School School { get; set; }
    public Semester Semester { get; set; }
}