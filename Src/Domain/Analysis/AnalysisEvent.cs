using Domain.Common;

namespace Domain.Analysis;

public sealed class AnalysisEvent : EntityEvent<int>
{
    private AnalysisEvent() { }

    public static AnalysisEvent Create(int entityId, string entityHash, Guid? createdBy = null)
    {
        AnalysisEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }

}
