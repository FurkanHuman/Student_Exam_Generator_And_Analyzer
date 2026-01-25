using Domain.Common;

namespace Domain.StudentAnswer;

public sealed class StudentAnswerEvent : EntityEvent<Guid>
{

    private StudentAnswerEvent() { }

    public static StudentAnswerEvent Create(Guid entityId, string entityHash, Guid? createdBy = null)
    {
        StudentAnswerEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
