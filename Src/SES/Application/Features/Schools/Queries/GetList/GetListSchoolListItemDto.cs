using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Schools.Queries.GetList;

public class GetListSchoolListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }

}