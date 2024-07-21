using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Personels;

public interface IPersonelService
{
    Task<Personel?> GetAsync(
        Expression<Func<Personel, bool>> predicate,
        Func<IQueryable<Personel>, IIncludableQueryable<Personel, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Personel>?> GetListAsync(
        Expression<Func<Personel, bool>>? predicate = null,
        Func<IQueryable<Personel>, IOrderedQueryable<Personel>>? orderBy = null,
        Func<IQueryable<Personel>, IIncludableQueryable<Personel, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Personel> AddAsync(Personel personel);
    Task<Personel> UpdateAsync(Personel personel);
    Task<Personel> DeleteAsync(Personel personel, bool permanent = false);
}
