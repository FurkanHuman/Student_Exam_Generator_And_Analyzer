namespace Application.Features.Students.CRUD.Create;

public record CreateStudentDTO
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public string SchoolNumber { get; set; }
    public char Gender { get; set; }
    public int ClassAge { get; set; }
    public char ClassBranch { get; set; }
    public int SchoolId { get; set; }
    public int SemesterId { get; set; }
}
