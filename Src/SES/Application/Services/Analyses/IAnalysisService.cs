using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Analyses;

public interface IAnalysisService
{
    Task<Analysis?> GetAsync(
        Expression<Func<Analysis, bool>> predicate,
        Func<IQueryable<Analysis>, IIncludableQueryable<Analysis, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Analysis>?> GetListAsync(
        Expression<Func<Analysis, bool>>? predicate = null,
        Func<IQueryable<Analysis>, IOrderedQueryable<Analysis>>? orderBy = null,
        Func<IQueryable<Analysis>, IIncludableQueryable<Analysis, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Analysis> AddAsync(Analysis analysis);
    Task<Analysis> UpdateAsync(Analysis analysis);
    Task<Analysis> DeleteAsync(Analysis analysis, bool permanent = false);
}
