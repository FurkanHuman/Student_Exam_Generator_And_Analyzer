using Application.Features.Benefits.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Benefits;

public class BenefitManager : IBenefitService
{
    private readonly IBenefitRepository _benefitRepository;
    private readonly BenefitBusinessRules _benefitBusinessRules;

    public BenefitManager(IBenefitRepository benefitRepository, BenefitBusinessRules benefitBusinessRules)
    {
        _benefitRepository = benefitRepository;
        _benefitBusinessRules = benefitBusinessRules;
    }

    public async Task<Benefit?> GetAsync(
        Expression<Func<Benefit, bool>> predicate,
        Func<IQueryable<Benefit>, IIncludableQueryable<Benefit, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Benefit? benefit = await _benefitRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return benefit;
    }

    public async Task<IPaginate<Benefit>?> GetListAsync(
        Expression<Func<Benefit, bool>>? predicate = null,
        Func<IQueryable<Benefit>, IOrderedQueryable<Benefit>>? orderBy = null,
        Func<IQueryable<Benefit>, IIncludableQueryable<Benefit, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Benefit> benefitList = await _benefitRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return benefitList;
    }

    public async Task<Benefit> AddAsync(Benefit benefit)
    {
        Benefit addedBenefit = await _benefitRepository.AddAsync(benefit);

        return addedBenefit;
    }

    public async Task<Benefit> UpdateAsync(Benefit benefit)
    {
        Benefit updatedBenefit = await _benefitRepository.UpdateAsync(benefit);

        return updatedBenefit;
    }

    public async Task<Benefit> DeleteAsync(Benefit benefit, bool permanent = false)
    {
        Benefit deletedBenefit = await _benefitRepository.DeleteAsync(benefit);

        return deletedBenefit;
    }
}
