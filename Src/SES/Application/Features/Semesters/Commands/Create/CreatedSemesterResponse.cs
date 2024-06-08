using NArchitecture.Core.Application.Responses;

namespace Application.Features.Semesters.Commands.Create;

public class CreatedSemesterResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BeginSemesterDate { get; set; }
    public DateOnly EndSemesterDate { get; set; }
}