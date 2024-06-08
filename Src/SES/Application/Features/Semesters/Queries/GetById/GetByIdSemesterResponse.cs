using NArchitecture.Core.Application.Responses;

namespace Application.Features.Semesters.Queries.GetById;

public class GetByIdSemesterResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BeginSemesterDate { get; set; }
    public DateOnly EndSemesterDate { get; set; }
}