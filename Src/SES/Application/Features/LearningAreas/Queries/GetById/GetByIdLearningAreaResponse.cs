using NArchitecture.Core.Application.Responses;

namespace Application.Features.LearningAreas.Queries.GetById;

public class GetByIdLearningAreaResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SubLearningAreaId { get; set; }
}