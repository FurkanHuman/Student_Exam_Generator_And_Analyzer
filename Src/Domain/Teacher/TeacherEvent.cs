using Domain.Common;

namespace Domain.Teacher;

public sealed class TeacherEvent : EntityEvent<int>
{
    private TeacherEvent() { }

    public static TeacherEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        TeacherEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
