using NArchitecture.Core.Application.Responses;

namespace Application.Features.Students.Commands.MultiCreate;
public class CreatedMultiStudentResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public int ClassAge { get; set; }
    public char Gender { get; set; }
    public char ClassBranch { get; set; }
    public string SchoolNumber { get; set; }
}
