using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Semesters.Queries.GetList;

public class GetListSemesterListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BeginSemesterDate { get; set; }
    public DateOnly EndSemesterDate { get; set; }
}