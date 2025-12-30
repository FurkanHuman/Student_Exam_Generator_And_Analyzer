using NArchitecture.Core.Application.Responses;

namespace Application.Features.Students.Commands.GhostCreate;

public class CreatedGhostStudentResponse : IResponse
{
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int FemaleCount { get; set; }
    public int MaleCount { get; set; }
    public List<NumberMapping> NumberMappings { get; set; } = [];
    public List<GhostStudentDto> Students { get; set; } = [];
}
