using NArchitecture.Core.Application.Responses;

namespace Application.Features.SubLearningAreas.Queries.GetById;

public class GetByIdSubLearningAreaResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BenefitId { get; set; }
}