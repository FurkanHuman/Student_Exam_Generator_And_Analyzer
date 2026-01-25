using Domain.Common;

namespace Domain.StudentAnswer;

public sealed class StudentExamAnswerEvent : EntityEvent<Guid>
{
    private StudentExamAnswerEvent() { }

    public static StudentExamAnswerEvent Create(Guid entityId, string entityHash, Guid? createdBy = null)
    {
        StudentExamAnswerEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
