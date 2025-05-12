using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentClasses.Queries.GetClassesBySemesterId;

public class GetClassesBySemesterIdResponse : IResponse 
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ClassAge { get; set; }
    public char ClassBranch { get; set; }
    public string? Decription { get; set; }
}
