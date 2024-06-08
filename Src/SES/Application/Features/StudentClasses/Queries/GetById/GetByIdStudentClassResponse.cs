using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentClasses.Queries.GetById;

public class GetByIdStudentClassResponse : IResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ClassAge { get; set; }
    public char ClassBranch { get; set; }
    public string? Decription { get; set; }
    public int SchoolId { get; set; }
    public int SemesterId { get; set; }
    public int RefTeacherId { get; set; }

}