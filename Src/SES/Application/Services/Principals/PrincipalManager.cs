using Application.Features.Principals.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.Principals;

public class PrincipalManager : IPrincipalService
{
    private readonly IPrincipalRepository _principalRepository;
    private readonly PrincipalBusinessRules _principalBusinessRules;

    public PrincipalManager(IPrincipalRepository principalRepository, PrincipalBusinessRules principalBusinessRules)
    {
        _principalRepository = principalRepository;
        _principalBusinessRules = principalBusinessRules;
    }

    public async Task<Principal?> GetAsync(
        Expression<Func<Principal, bool>> predicate,
        Func<IQueryable<Principal>, IIncludableQueryable<Principal, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Principal? principal = await _principalRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return principal;
    }

    public async Task<IPaginate<Principal>?> GetListAsync(
        Expression<Func<Principal, bool>>? predicate = null,
        Func<IQueryable<Principal>, IOrderedQueryable<Principal>>? orderBy = null,
        Func<IQueryable<Principal>, IIncludableQueryable<Principal, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Principal> principalList = await _principalRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return principalList;
    }

    public async Task<Principal> AddAsync(Principal principal)
    {
        Principal addedPrincipal = await _principalRepository.AddAsync(principal);

        return addedPrincipal;
    }

    public async Task<Principal> UpdateAsync(Principal principal)
    {
        Principal updatedPrincipal = await _principalRepository.UpdateAsync(principal);

        return updatedPrincipal;
    }

    public async Task<Principal> DeleteAsync(Principal principal, bool permanent = false)
    {
        Principal deletedPrincipal = await _principalRepository.DeleteAsync(principal);

        return deletedPrincipal;
    }
}
