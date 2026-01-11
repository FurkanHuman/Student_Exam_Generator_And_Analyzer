using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.FeedBacks;

public interface IFeedBackService
{
    Task<FeedBack?> GetAsync(
        Expression<Func<FeedBack, bool>> predicate,
        Func<IQueryable<FeedBack>, IIncludableQueryable<FeedBack, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<FeedBack>?> GetListAsync(
        Expression<Func<FeedBack, bool>>? predicate = null,
        Func<IQueryable<FeedBack>, IOrderedQueryable<FeedBack>>? orderBy = null,
        Func<IQueryable<FeedBack>, IIncludableQueryable<FeedBack, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<FeedBack> AddAsync(FeedBack feedBack);
    Task<FeedBack> UpdateAsync(FeedBack feedBack);
    Task<FeedBack> DeleteAsync(FeedBack feedBack, bool permanent = false);
}
