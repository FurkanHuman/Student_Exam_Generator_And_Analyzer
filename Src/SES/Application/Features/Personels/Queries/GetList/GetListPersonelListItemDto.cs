using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Personels.Queries.GetList;

public class GetListPersonelListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public DateOnly BirthDate { get; set; }
}