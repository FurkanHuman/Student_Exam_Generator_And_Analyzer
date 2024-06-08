using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.LearningAreas;

public interface ILearningAreaService
{
    Task<LearningArea?> GetAsync(
        Expression<Func<LearningArea, bool>> predicate,
        Func<IQueryable<LearningArea>, IIncludableQueryable<LearningArea, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<LearningArea>?> GetListAsync(
        Expression<Func<LearningArea, bool>>? predicate = null,
        Func<IQueryable<LearningArea>, IOrderedQueryable<LearningArea>>? orderBy = null,
        Func<IQueryable<LearningArea>, IIncludableQueryable<LearningArea, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<LearningArea> AddAsync(LearningArea learningArea);
    Task<LearningArea> UpdateAsync(LearningArea learningArea);
    Task<LearningArea> DeleteAsync(LearningArea learningArea, bool permanent = false);
}
