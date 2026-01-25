using Domain.Common;

namespace Domain.LearningArea;

public sealed class LearningAreaEvent : EntityEvent<int>
{
    private LearningAreaEvent() { }

    public static LearningAreaEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        LearningAreaEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
