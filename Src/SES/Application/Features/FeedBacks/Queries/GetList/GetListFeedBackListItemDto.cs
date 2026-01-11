using NArchitecture.Core.Application.Dtos;

namespace Application.Features.FeedBacks.Queries.GetList;

public class GetListFeedBackListItemDto : IDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string Email { get; set; }
    public string PageUrl { get; set; }
    public string Message { get; set; }
    public DateTime SubmittedAt { get; set; }
}