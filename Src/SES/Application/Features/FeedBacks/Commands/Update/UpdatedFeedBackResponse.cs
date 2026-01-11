using NArchitecture.Core.Application.Responses;

namespace Application.Features.FeedBacks.Commands.Update;

public class UpdatedFeedBackResponse : IResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string Email { get; set; }
    public string PageUrl { get; set; }
    public string Message { get; set; }
    public DateTime SubmittedAt { get; set; }
}