using Domain.Common;

namespace Domain.Principal;

public sealed class PrincipalEvent : EntityEvent<int>
{
    private PrincipalEvent() { }

    public static PrincipalEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        PrincipalEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
