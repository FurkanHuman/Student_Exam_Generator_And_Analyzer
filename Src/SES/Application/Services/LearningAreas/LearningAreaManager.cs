using Application.Features.LearningAreas.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.LearningAreas;

public class LearningAreaManager : ILearningAreaService
{
    private readonly ILearningAreaRepository _learningAreaRepository;
    private readonly LearningAreaBusinessRules _learningAreaBusinessRules;

    public LearningAreaManager(ILearningAreaRepository learningAreaRepository, LearningAreaBusinessRules learningAreaBusinessRules)
    {
        _learningAreaRepository = learningAreaRepository;
        _learningAreaBusinessRules = learningAreaBusinessRules;
    }

    public async Task<LearningArea?> GetAsync(
        Expression<Func<LearningArea, bool>> predicate,
        Func<IQueryable<LearningArea>, IIncludableQueryable<LearningArea, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        LearningArea? learningArea = await _learningAreaRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return learningArea;
    }

    public async Task<IPaginate<LearningArea>?> GetListAsync(
        Expression<Func<LearningArea, bool>>? predicate = null,
        Func<IQueryable<LearningArea>, IOrderedQueryable<LearningArea>>? orderBy = null,
        Func<IQueryable<LearningArea>, IIncludableQueryable<LearningArea, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<LearningArea> learningAreaList = await _learningAreaRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return learningAreaList;
    }

    public async Task<LearningArea> AddAsync(LearningArea learningArea)
    {
        LearningArea addedLearningArea = await _learningAreaRepository.AddAsync(learningArea);

        return addedLearningArea;
    }

    public async Task<LearningArea> UpdateAsync(LearningArea learningArea)
    {
        LearningArea updatedLearningArea = await _learningAreaRepository.UpdateAsync(learningArea);

        return updatedLearningArea;
    }

    public async Task<LearningArea> DeleteAsync(LearningArea learningArea, bool permanent = false)
    {
        LearningArea deletedLearningArea = await _learningAreaRepository.DeleteAsync(learningArea);

        return deletedLearningArea;
    }
}
