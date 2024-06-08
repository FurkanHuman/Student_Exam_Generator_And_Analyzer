using NArchitecture.Core.Application.Dtos;

namespace Application.Features.LearningAreas.Queries.GetList;

public class GetListLearningAreaListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SubLearningAreaId { get; set; }
}