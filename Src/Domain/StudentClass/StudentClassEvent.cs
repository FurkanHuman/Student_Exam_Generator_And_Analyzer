using Domain.Common;

namespace Domain.StudentClass;

public sealed class StudentClassEvent : EntityEvent<int>
{
    private StudentClassEvent() { }

    public static StudentClassEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        StudentClassEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
