using Application.Features.SubLearningAreas.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.SubLearningAreas;

public class SubLearningAreaManager : ISubLearningAreaService
{
    private readonly ISubLearningAreaRepository _subLearningAreaRepository;
    private readonly SubLearningAreaBusinessRules _subLearningAreaBusinessRules;

    public SubLearningAreaManager(ISubLearningAreaRepository subLearningAreaRepository, SubLearningAreaBusinessRules subLearningAreaBusinessRules)
    {
        _subLearningAreaRepository = subLearningAreaRepository;
        _subLearningAreaBusinessRules = subLearningAreaBusinessRules;
    }

    public async Task<SubLearningArea?> GetAsync(
        Expression<Func<SubLearningArea, bool>> predicate,
        Func<IQueryable<SubLearningArea>, IIncludableQueryable<SubLearningArea, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        SubLearningArea? subLearningArea = await _subLearningAreaRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return subLearningArea;
    }

    public async Task<IPaginate<SubLearningArea>?> GetListAsync(
        Expression<Func<SubLearningArea, bool>>? predicate = null,
        Func<IQueryable<SubLearningArea>, IOrderedQueryable<SubLearningArea>>? orderBy = null,
        Func<IQueryable<SubLearningArea>, IIncludableQueryable<SubLearningArea, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<SubLearningArea> subLearningAreaList = await _subLearningAreaRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return subLearningAreaList;
    }

    public async Task<SubLearningArea> AddAsync(SubLearningArea subLearningArea)
    {
        SubLearningArea addedSubLearningArea = await _subLearningAreaRepository.AddAsync(subLearningArea);

        return addedSubLearningArea;
    }

    public async Task<SubLearningArea> UpdateAsync(SubLearningArea subLearningArea)
    {
        SubLearningArea updatedSubLearningArea = await _subLearningAreaRepository.UpdateAsync(subLearningArea);

        return updatedSubLearningArea;
    }

    public async Task<SubLearningArea> DeleteAsync(SubLearningArea subLearningArea, bool permanent = false)
    {
        SubLearningArea deletedSubLearningArea = await _subLearningAreaRepository.DeleteAsync(subLearningArea);

        return deletedSubLearningArea;
    }
}
