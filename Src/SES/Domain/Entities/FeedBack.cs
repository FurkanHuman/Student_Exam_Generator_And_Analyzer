using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class FeedBack: Entity<Guid>
{
    public string? UserName { get; set; }
    public string Email { get; set; }
    public string PageUrl { get; set; }
    public string Message { get; set; }
    public DateTime SubmittedAt { get; set; }
}
