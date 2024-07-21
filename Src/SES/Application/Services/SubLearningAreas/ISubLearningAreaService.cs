using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.SubLearningAreas;

public interface ISubLearningAreaService
{
    Task<SubLearningArea?> GetAsync(
        Expression<Func<SubLearningArea, bool>> predicate,
        Func<IQueryable<SubLearningArea>, IIncludableQueryable<SubLearningArea, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<SubLearningArea>?> GetListAsync(
        Expression<Func<SubLearningArea, bool>>? predicate = null,
        Func<IQueryable<SubLearningArea>, IOrderedQueryable<SubLearningArea>>? orderBy = null,
        Func<IQueryable<SubLearningArea>, IIncludableQueryable<SubLearningArea, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<SubLearningArea> AddAsync(SubLearningArea subLearningArea);
    Task<SubLearningArea> UpdateAsync(SubLearningArea subLearningArea);
    Task<SubLearningArea> DeleteAsync(SubLearningArea subLearningArea, bool permanent = false);
}
