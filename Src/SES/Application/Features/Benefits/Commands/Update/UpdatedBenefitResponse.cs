using NArchitecture.Core.Application.Responses;

namespace Application.Features.Benefits.Commands.Update;

public class UpdatedBenefitResponse : IResponse
{
    public int Id { get; set; }
    public int SubLearningAreaId { get; set; }
    public string ReferenceBenefitNumber { get; set; }
    public string ReferenceBenefitComments { get; set; }
}