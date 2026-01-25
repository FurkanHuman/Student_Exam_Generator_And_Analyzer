using Domain.Common;

namespace Domain.ReferenceBenefit;

public sealed class ReferenceBenefitEvent : EntityEvent<int>
{
    private ReferenceBenefitEvent() { }
    public static ReferenceBenefitEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        ReferenceBenefitEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
