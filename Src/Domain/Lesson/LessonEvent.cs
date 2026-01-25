using Domain.Common;

namespace Domain.Lesson;

public sealed class LessonEvent : EntityEvent<int>
{
    private LessonEvent() { }

    public static LessonEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        LessonEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
