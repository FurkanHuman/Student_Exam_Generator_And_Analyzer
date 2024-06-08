using NArchitecture.Core.Application.Dtos;

namespace Application.Features.SubLearningAreas.Queries.GetList;

public class GetListSubLearningAreaListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BenefitId { get; set; }
}