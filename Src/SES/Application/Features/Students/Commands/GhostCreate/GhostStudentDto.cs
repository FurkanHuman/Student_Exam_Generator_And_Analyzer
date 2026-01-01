namespace Application.Features.Students.Commands.GhostCreate;

public class GhostStudentDto
{
    public int Id { get; set; }
    public string SchoolNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public char Gender { get; set; }
}