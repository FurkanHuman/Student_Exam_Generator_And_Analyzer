using NArchitecture.Core.Application.Responses;

namespace Application.Features.FeedBacks.Queries.GetById;

public class GetByIdFeedBackResponse : IResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string Email { get; set; }
    public string PageUrl { get; set; }
    public string Message { get; set; }
    public DateTime SubmittedAt { get; set; }
}