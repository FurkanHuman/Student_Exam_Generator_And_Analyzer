using Domain.Common;

namespace Domain.Personel;

public sealed class PersonelEvent : EntityEvent<Guid>
{
    private PersonelEvent() { }

    public static PersonelEvent Create(Guid entityId, string entityHash, Guid? createdBy = null)
    {
        PersonelEvent evt = new();
        evt.InitializeEvent(entityId, entityHash, createdBy);
        return evt;
    }

}
