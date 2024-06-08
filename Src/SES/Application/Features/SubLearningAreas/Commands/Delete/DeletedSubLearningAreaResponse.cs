using NArchitecture.Core.Application.Responses;

namespace Application.Features.SubLearningAreas.Commands.Delete;

public class DeletedSubLearningAreaResponse : IResponse
{
    public int Id { get; set; }
}