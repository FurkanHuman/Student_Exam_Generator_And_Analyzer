using NArchitecture.Core.Application.Responses;

namespace Application.Features.SubLearningAreas.Commands.Update;

public class UpdatedSubLearningAreaResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BenefitId { get; set; }
}