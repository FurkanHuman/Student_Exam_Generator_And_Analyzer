using Domain.Common;

namespace Domain.Question;

public sealed class QuizQuestionEvent : EntityEvent<int>
{
    private QuizQuestionEvent() { }

    public static QuizQuestionEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        QuizQuestionEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }
}
