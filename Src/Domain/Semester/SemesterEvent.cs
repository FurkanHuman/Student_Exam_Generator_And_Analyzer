using Domain.Common;

namespace Domain.Semester;

public sealed class SemesterEvent : EntityEvent<int>
{
    private SemesterEvent() { }

    public static SemesterEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        SemesterEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }

}
