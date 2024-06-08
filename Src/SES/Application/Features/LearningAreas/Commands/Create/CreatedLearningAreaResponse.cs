using NArchitecture.Core.Application.Responses;

namespace Application.Features.LearningAreas.Commands.Create;

public class CreatedLearningAreaResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SubLearningAreaId { get; set; }
}