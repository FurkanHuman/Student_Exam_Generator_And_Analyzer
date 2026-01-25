using Domain.Common;

namespace Domain.Exam;

public sealed class ExamConfigurationEvent : EntityEvent<int>
{
    private ExamConfigurationEvent() { }

    public static ExamConfigurationEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        ExamConfigurationEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
