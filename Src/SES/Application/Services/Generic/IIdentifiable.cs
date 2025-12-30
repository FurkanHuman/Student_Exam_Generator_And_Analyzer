using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Generic;

public interface IIdentifiable<TEntity, TId>
    where TEntity : Entity<TId>, new()
    where TId : struct
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
}
