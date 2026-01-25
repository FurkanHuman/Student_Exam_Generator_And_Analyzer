using Domain.Common;

namespace Domain.LearningArea;

public sealed class SubLearningAreaEvent : EntityEvent<int>
{
    private SubLearningAreaEvent() { }

    public static SubLearningAreaEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        SubLearningAreaEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
