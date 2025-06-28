using NArchitecture.Core.Application.Responses;

namespace Application.Features.StudentClasses.Queries.GetClassesByClassAge;

public class GetClassesByClassAgeResponse : IResponse
{
    public int Id { get; set; }
    public char ClassBranch { get; set; }
    public string? ClassName { get; set; }
}
