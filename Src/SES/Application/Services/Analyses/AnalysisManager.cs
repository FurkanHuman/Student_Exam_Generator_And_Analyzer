using Application.Features.Analyses.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Analyses;

public class AnalysisManager : IAnalysisService
{
    private readonly IAnalysisRepository _analysisRepository;
    private readonly AnalysisBusinessRules _analysisBusinessRules;

    public AnalysisManager(IAnalysisRepository analysisRepository, AnalysisBusinessRules analysisBusinessRules)
    {
        _analysisRepository = analysisRepository;
        _analysisBusinessRules = analysisBusinessRules;
    }

    public async Task<Analysis?> GetAsync(
        Expression<Func<Analysis, bool>> predicate,
        Func<IQueryable<Analysis>, IIncludableQueryable<Analysis, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Analysis? analysis = await _analysisRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return analysis;
    }

    public async Task<IPaginate<Analysis>?> GetListAsync(
        Expression<Func<Analysis, bool>>? predicate = null,
        Func<IQueryable<Analysis>, IOrderedQueryable<Analysis>>? orderBy = null,
        Func<IQueryable<Analysis>, IIncludableQueryable<Analysis, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Analysis> analysisList = await _analysisRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return analysisList;
    }

    public async Task<Analysis> AddAsync(Analysis analysis)
    {
        Analysis addedAnalysis = await _analysisRepository.AddAsync(analysis);

        return addedAnalysis;
    }

    public async Task<Analysis> UpdateAsync(Analysis analysis)
    {
        Analysis updatedAnalysis = await _analysisRepository.UpdateAsync(analysis);

        return updatedAnalysis;
    }

    public async Task<Analysis> DeleteAsync(Analysis analysis, bool permanent = false)
    {
        Analysis deletedAnalysis = await _analysisRepository.DeleteAsync(analysis);

        return deletedAnalysis;
    }
}
