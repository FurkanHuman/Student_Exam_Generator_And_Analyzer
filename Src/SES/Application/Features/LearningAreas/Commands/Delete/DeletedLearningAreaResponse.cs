using NArchitecture.Core.Application.Responses;

namespace Application.Features.LearningAreas.Commands.Delete;

public class DeletedLearningAreaResponse : IResponse
{
    public int Id { get; set; }
}