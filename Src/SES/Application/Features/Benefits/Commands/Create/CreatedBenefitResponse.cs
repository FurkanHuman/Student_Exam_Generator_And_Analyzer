using NArchitecture.Core.Application.Responses;

namespace Application.Features.Benefits.Commands.Create;

public class CreatedBenefitResponse : IResponse
{
    public int Id { get; set; }
    public int SubLearningAreaId { get; set; }
    public string ReferenceBenefitNumber { get; set; }
    public string ReferenceBenefitComments { get; set; }
}