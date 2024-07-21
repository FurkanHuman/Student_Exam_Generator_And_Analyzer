using Application.Features.ReferenceBenefits.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.ReferenceBenefits;

public class ReferenceBenefitManager : IReferenceBenefitService
{
    private readonly IReferenceBenefitRepository _referenceBenefitRepository;
    private readonly ReferenceBenefitBusinessRules _referenceBenefitBusinessRules;

    public ReferenceBenefitManager(IReferenceBenefitRepository referenceBenefitRepository, ReferenceBenefitBusinessRules referenceBenefitBusinessRules)
    {
        _referenceBenefitRepository = referenceBenefitRepository;
        _referenceBenefitBusinessRules = referenceBenefitBusinessRules;
    }

    public async Task<ReferenceBenefit?> GetAsync(
        Expression<Func<ReferenceBenefit, bool>> predicate,
        Func<IQueryable<ReferenceBenefit>, IIncludableQueryable<ReferenceBenefit, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        ReferenceBenefit? referenceBenefit = await _referenceBenefitRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return referenceBenefit;
    }

    public async Task<IPaginate<ReferenceBenefit>?> GetListAsync(
        Expression<Func<ReferenceBenefit, bool>>? predicate = null,
        Func<IQueryable<ReferenceBenefit>, IOrderedQueryable<ReferenceBenefit>>? orderBy = null,
        Func<IQueryable<ReferenceBenefit>, IIncludableQueryable<ReferenceBenefit, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<ReferenceBenefit> referenceBenefitList = await _referenceBenefitRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return referenceBenefitList;
    }

    public async Task<ReferenceBenefit> AddAsync(ReferenceBenefit referenceBenefit)
    {
        ReferenceBenefit addedReferenceBenefit = await _referenceBenefitRepository.AddAsync(referenceBenefit);

        return addedReferenceBenefit;
    }

    public async Task<ReferenceBenefit> UpdateAsync(ReferenceBenefit referenceBenefit)
    {
        ReferenceBenefit updatedReferenceBenefit = await _referenceBenefitRepository.UpdateAsync(referenceBenefit);

        return updatedReferenceBenefit;
    }

    public async Task<ReferenceBenefit> DeleteAsync(ReferenceBenefit referenceBenefit, bool permanent = false)
    {
        ReferenceBenefit deletedReferenceBenefit = await _referenceBenefitRepository.DeleteAsync(referenceBenefit);

        return deletedReferenceBenefit;
    }
}
