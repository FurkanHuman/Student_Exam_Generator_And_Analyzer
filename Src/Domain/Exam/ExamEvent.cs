using Domain.Common;

namespace Domain.Exam;

public sealed class ExamEvent : EntityEvent<int>
{
    private ExamEvent() { }

    public static ExamEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        ExamEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
