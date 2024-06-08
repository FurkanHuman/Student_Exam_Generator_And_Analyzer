using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Benefits;

public interface IBenefitService
{
    Task<Benefit?> GetAsync(
        Expression<Func<Benefit, bool>> predicate,
        Func<IQueryable<Benefit>, IIncludableQueryable<Benefit, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Benefit>?> GetListAsync(
        Expression<Func<Benefit, bool>>? predicate = null,
        Func<IQueryable<Benefit>, IOrderedQueryable<Benefit>>? orderBy = null,
        Func<IQueryable<Benefit>, IIncludableQueryable<Benefit, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Benefit> AddAsync(Benefit benefit);
    Task<Benefit> UpdateAsync(Benefit benefit);
    Task<Benefit> DeleteAsync(Benefit benefit, bool permanent = false);
}
