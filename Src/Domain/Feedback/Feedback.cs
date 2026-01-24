using Domain.Common;

namespace Domain.Feedback;

public sealed class Feedback : Entity<Guid>
{
    public string? UserName { get; private set; }
    public string Email { get; private set; }
    public string PageUrl { get; private set; }
    public string Message { get; private set; }
    public DateTime SubmittedAt { get; private set; }

    private Feedback()
    {
        Email = string.Empty;
        PageUrl = string.Empty;
        Message = string.Empty;
    }

    public static Feedback Create(string email, string pageUrl, string message, string? userName = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");

        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("Message cannot be empty");

        return new Feedback
        {
            Id = Guid.NewGuid(),
            UserName = userName?.Trim(),
            Email = email.Trim().ToLower(),
            PageUrl = pageUrl.Trim(),
            Message = message.Trim(),
            SubmittedAt = DateTime.UtcNow
        };
    }
}
