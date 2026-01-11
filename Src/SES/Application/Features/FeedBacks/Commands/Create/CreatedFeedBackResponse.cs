using NArchitecture.Core.Application.Responses;

namespace Application.Features.FeedBacks.Commands.Create;

public class CreatedFeedBackResponse : IResponse
{
    public bool Success { get; set; }
}