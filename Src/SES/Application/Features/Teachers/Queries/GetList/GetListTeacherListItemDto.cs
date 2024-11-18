using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Teachers.Queries.GetList;

public class GetListTeacherListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
}