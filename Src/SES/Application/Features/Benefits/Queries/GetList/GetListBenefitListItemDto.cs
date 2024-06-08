using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Benefits.Queries.GetList;

public class GetListBenefitListItemDto : IDto
{
    public int Id { get; set; }
    public int SubLearningAreaId { get; set; }
    public string ReferenceBenefitNumber { get; set; }
    public string ReferenceBenefitComments { get; set; }
}