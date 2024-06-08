using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Semesters;

public interface ISemesterService
{
    Task<Semester?> GetAsync(
        Expression<Func<Semester, bool>> predicate,
        Func<IQueryable<Semester>, IIncludableQueryable<Semester, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Semester>?> GetListAsync(
        Expression<Func<Semester, bool>>? predicate = null,
        Func<IQueryable<Semester>, IOrderedQueryable<Semester>>? orderBy = null,
        Func<IQueryable<Semester>, IIncludableQueryable<Semester, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Semester> AddAsync(Semester semester);
    Task<Semester> UpdateAsync(Semester semester);
    Task<Semester> DeleteAsync(Semester semester, bool permanent = false);
}
