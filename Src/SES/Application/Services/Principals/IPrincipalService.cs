using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Principals;

public interface IPrincipalService
{
    Task<Principal?> GetAsync(
        Expression<Func<Principal, bool>> predicate,
        Func<IQueryable<Principal>, IIncludableQueryable<Principal, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Principal>?> GetListAsync(
        Expression<Func<Principal, bool>>? predicate = null,
        Func<IQueryable<Principal>, IOrderedQueryable<Principal>>? orderBy = null,
        Func<IQueryable<Principal>, IIncludableQueryable<Principal, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Principal> AddAsync(Principal principal);
    Task<Principal> UpdateAsync(Principal principal);
    Task<Principal> DeleteAsync(Principal principal, bool permanent = false);
}
