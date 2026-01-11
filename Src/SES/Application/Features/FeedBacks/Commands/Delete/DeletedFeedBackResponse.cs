using NArchitecture.Core.Application.Responses;

namespace Application.Features.FeedBacks.Commands.Delete;

public class DeletedFeedBackResponse : IResponse
{
    public Guid Id { get; set; }
}