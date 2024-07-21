using Application.Features.Personels.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Personels;

public class PersonelManager : IPersonelService
{
    private readonly IPersonelRepository _personelRepository;
    private readonly PersonelBusinessRules _personelBusinessRules;

    public PersonelManager(IPersonelRepository personelRepository, PersonelBusinessRules personelBusinessRules)
    {
        _personelRepository = personelRepository;
        _personelBusinessRules = personelBusinessRules;
    }

    public async Task<Personel?> GetAsync(
        Expression<Func<Personel, bool>> predicate,
        Func<IQueryable<Personel>, IIncludableQueryable<Personel, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Personel? personel = await _personelRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return personel;
    }

    public async Task<IPaginate<Personel>?> GetListAsync(
        Expression<Func<Personel, bool>>? predicate = null,
        Func<IQueryable<Personel>, IOrderedQueryable<Personel>>? orderBy = null,
        Func<IQueryable<Personel>, IIncludableQueryable<Personel, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Personel> personelList = await _personelRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return personelList;
    }

    public async Task<Personel> AddAsync(Personel personel)
    {
        Personel addedPersonel = await _personelRepository.AddAsync(personel);

        return addedPersonel;
    }

    public async Task<Personel> UpdateAsync(Personel personel)
    {
        Personel updatedPersonel = await _personelRepository.UpdateAsync(personel);

        return updatedPersonel;
    }

    public async Task<Personel> DeleteAsync(Personel personel, bool permanent = false)
    {
        Personel deletedPersonel = await _personelRepository.DeleteAsync(personel);

        return deletedPersonel;
    }
}
