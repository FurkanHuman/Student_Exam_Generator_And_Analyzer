
using Entity.Base;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataAccess.Core;

public interface IRepository<TEntity, TEntityId> : IQuery<TEntity>
    where TEntity : Entity<TEntityId>,new()
    where TEntityId : struct
{
    TEntity? Get(
        Expression<Func<TEntity, bool>> peredicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null
        );

    IList<TEntity> GetList(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

    bool Any(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true);
    TEntity Add(TEntity entity);
    ICollection<TEntity> AddRange(ICollection<TEntity> entities);
    TEntity Update(TEntity entity);
    ICollection<TEntity> UpdateRange(ICollection<TEntity> entities);
    TEntity Delete(TEntity entity, bool permanent = false);
    ICollection<TEntity> DeleteRangeAsync(ICollection<TEntity> entity, bool permanent = false);
}