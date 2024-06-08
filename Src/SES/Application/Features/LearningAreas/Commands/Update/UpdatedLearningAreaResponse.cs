using NArchitecture.Core.Application.Responses;

namespace Application.Features.LearningAreas.Commands.Update;

public class UpdatedLearningAreaResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SubLearningAreaId { get; set; }
}