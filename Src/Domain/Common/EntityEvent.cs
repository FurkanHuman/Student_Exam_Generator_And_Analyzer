using Domain.Common.ValueObjects;

namespace Domain.Common;

public abstract class EntityEvent<TId> : Entity<TId> where TId : notnull
{
    public TId EntityId { get; protected set; } = default!;

    public AuditMetadata Audit { get; protected set; } = null!;

    public EntityHash Hash { get; protected set; } = null!;

    protected EntityEvent() { }

    protected void InitializeEvent(TId entityId, string initialEntityHash, Guid? createdBy = null)
    {
        EntityId = entityId;
        Audit = AuditMetadata.Create(createdBy);
        Hash = EntityHash.Compute(initialEntityHash);
    }

    public void RecordUpdate(string newEntityHash, Guid? modifiedBy = null)
    {
        Audit = Audit.MarkUpdated(modifiedBy);
        Hash = EntityHash.Compute(newEntityHash);
    }

    public void RecordDeletion(string finalEntityHash, Guid? deletedBy = null)
    {
        Audit = Audit.MarkDeleted(deletedBy);
        Hash = EntityHash.Compute(finalEntityHash);
    }

    public void RecordRestoration(string restoredEntityHash, Guid? restoredBy = null)
    {
        Audit = Audit.Restore(restoredBy);
        Hash = EntityHash.Compute(restoredEntityHash);
    }
}
