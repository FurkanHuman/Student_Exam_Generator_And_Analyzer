
using Entity.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataAccess.Core;

public class EfRepositoryBase<TEntity, TEntityId, TContext>(TContext context) : IAsyncRepository<TEntity, TEntityId>, IRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>, new()
    where TEntityId : struct
    where TContext : DbContext
{
    protected readonly TContext Context = context;

    public TEntity Add(TEntity entity)
    {
        Context.Add(entity);
        Context.SaveChanges();
        return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
    {        
            Context.AddRange(entities);
            Context.SaveChanges();
            return entities;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        await Context.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
        return entities;
    }

    public bool Any(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.Any();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.AnyAsync(cancellationToken);

    }

    public TEntity Delete(TEntity entity, bool permanent = false)
    {
        Context.Remove(entity);
        Context.SaveChanges();
        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entity, bool permanent = false)
    {
        Context.RemoveRange(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entity, bool permanent = false)
    {
        throw new NotImplementedException();
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> peredicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IList<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
    {
        throw new NotImplementedException();
    }

    public Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> Query() => Context.Set<TEntity>();
    public TEntity Update(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entity)
    {
        throw new NotImplementedException();
    }
}
