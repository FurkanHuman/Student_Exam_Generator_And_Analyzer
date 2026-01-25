using Domain.Common;

namespace Domain.Feedback;

public sealed class FeedbackEvent : EntityEvent<Guid>
{
    private FeedbackEvent() { }

    public static FeedbackEvent Create(Guid entityId, string entityHash, Guid? createdBy = null)
    {
        FeedbackEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
