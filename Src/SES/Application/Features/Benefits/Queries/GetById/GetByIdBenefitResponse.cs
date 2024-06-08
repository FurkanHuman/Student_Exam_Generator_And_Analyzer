using NArchitecture.Core.Application.Responses;

namespace Application.Features.Benefits.Queries.GetById;

public class GetByIdBenefitResponse : IResponse
{
    public int Id { get; set; }
    public int SubLearningAreaId { get; set; }
    public string ReferenceBenefitNumber { get; set; }
    public string ReferenceBenefitComments { get; set; }
}