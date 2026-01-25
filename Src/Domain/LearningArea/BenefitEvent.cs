using Domain.Common;

namespace Domain.LearningArea;

public sealed class BenefitEvent : EntityEvent<int>
{
    private BenefitEvent() { }

    public static BenefitEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        BenefitEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
