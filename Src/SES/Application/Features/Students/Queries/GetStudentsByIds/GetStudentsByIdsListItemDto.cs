using NArchitecture.Core.Application.Dtos;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Students.Queries.GetStudentsByIds;

public class GetStudentsByIdsListItemDto : IResponse, IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string SchoolNumber { get; set; }
}
