using Application.Features.FeedBacks.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.FeedBacks;

public class FeedBackManager : IFeedBackService
{
    private readonly IFeedBackRepository _feedBackRepository;
    private readonly FeedBackBusinessRules _feedBackBusinessRules;

    public FeedBackManager(IFeedBackRepository feedBackRepository, FeedBackBusinessRules feedBackBusinessRules)
    {
        _feedBackRepository = feedBackRepository;
        _feedBackBusinessRules = feedBackBusinessRules;
    }

    public async Task<FeedBack?> GetAsync(
        Expression<Func<FeedBack, bool>> predicate,
        Func<IQueryable<FeedBack>, IIncludableQueryable<FeedBack, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        FeedBack? feedBack = await _feedBackRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return feedBack;
    }

    public async Task<IPaginate<FeedBack>?> GetListAsync(
        Expression<Func<FeedBack, bool>>? predicate = null,
        Func<IQueryable<FeedBack>, IOrderedQueryable<FeedBack>>? orderBy = null,
        Func<IQueryable<FeedBack>, IIncludableQueryable<FeedBack, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<FeedBack> feedBackList = await _feedBackRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return feedBackList;
    }

    public async Task<FeedBack> AddAsync(FeedBack feedBack)
    {
        FeedBack addedFeedBack = await _feedBackRepository.AddAsync(feedBack);

        return addedFeedBack;
    }

    public async Task<FeedBack> UpdateAsync(FeedBack feedBack)
    {
        FeedBack updatedFeedBack = await _feedBackRepository.UpdateAsync(feedBack);

        return updatedFeedBack;
    }

    public async Task<FeedBack> DeleteAsync(FeedBack feedBack, bool permanent = false)
    {
        FeedBack deletedFeedBack = await _feedBackRepository.DeleteAsync(feedBack);

        return deletedFeedBack;
    }
}
