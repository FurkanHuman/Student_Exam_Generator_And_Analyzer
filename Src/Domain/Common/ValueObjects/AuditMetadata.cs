namespace Domain.Common.ValueObjects;

public sealed record AuditMetadata : ValueObject
{
    public DateTime CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public DateTime? DeletedDate { get; init; }
    public Guid? CreatedBy { get; init; }
    public Guid? ModifiedBy { get; init; }
    public bool IsDeleted => DeletedDate.HasValue;

    private AuditMetadata() { }

    private AuditMetadata(DateTime createdDate, Guid? createdBy)
    {
        CreatedDate = createdDate;
        CreatedBy = createdBy;
    }

    public static AuditMetadata Create(Guid? createdBy = null) => new(DateTime.UtcNow, createdBy);

    public AuditMetadata MarkUpdated(Guid? modifiedBy = null)
    {
        return this with
        {
            UpdatedDate = DateTime.UtcNow,
            ModifiedBy = modifiedBy
        };
    }

    public AuditMetadata MarkDeleted(Guid? deletedBy = null)
    {
        return this with
        {
            DeletedDate = DateTime.UtcNow,
            ModifiedBy = deletedBy
        };
    }

    public AuditMetadata Restore(Guid? restoredBy = null)
    {
        return this with
        {
            DeletedDate = null,
            UpdatedDate = DateTime.UtcNow,
            ModifiedBy = restoredBy
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedDate;
        yield return UpdatedDate;
        yield return DeletedDate;
        yield return CreatedBy;
        yield return ModifiedBy;
    }
}
