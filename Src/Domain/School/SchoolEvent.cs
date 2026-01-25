using Domain.Common;

namespace Domain.School;

public sealed class SchoolEvent : EntityEvent<int>
{
    private SchoolEvent() { }

    public static SchoolEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        SchoolEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
