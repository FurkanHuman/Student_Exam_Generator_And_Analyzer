using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.ReferenceBenefits;

public interface IReferenceBenefitService
{
    Task<ReferenceBenefit?> GetAsync(
        Expression<Func<ReferenceBenefit, bool>> predicate,
        Func<IQueryable<ReferenceBenefit>, IIncludableQueryable<ReferenceBenefit, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<ReferenceBenefit>?> GetListAsync(
        Expression<Func<ReferenceBenefit, bool>>? predicate = null,
        Func<IQueryable<ReferenceBenefit>, IOrderedQueryable<ReferenceBenefit>>? orderBy = null,
        Func<IQueryable<ReferenceBenefit>, IIncludableQueryable<ReferenceBenefit, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<ReferenceBenefit> AddAsync(ReferenceBenefit referenceBenefit);
    Task<ReferenceBenefit> UpdateAsync(ReferenceBenefit referenceBenefit);
    Task<ReferenceBenefit> DeleteAsync(ReferenceBenefit referenceBenefit, bool permanent = false);
}
