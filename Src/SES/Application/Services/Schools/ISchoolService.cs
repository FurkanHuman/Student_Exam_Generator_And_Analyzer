using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Schools;

public interface ISchoolService
{
    Task<School?> GetAsync(
        Expression<Func<School, bool>> predicate,
        Func<IQueryable<School>, IIncludableQueryable<School, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<School>?> GetListAsync(
        Expression<Func<School, bool>>? predicate = null,
        Func<IQueryable<School>, IOrderedQueryable<School>>? orderBy = null,
        Func<IQueryable<School>, IIncludableQueryable<School, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<School> AddAsync(School school);
    Task<School> UpdateAsync(School school);
    Task<School> DeleteAsync(School school, bool permanent = false);
}
