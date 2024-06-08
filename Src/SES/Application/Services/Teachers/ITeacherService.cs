using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Teachers;

public interface ITeacherService
{
    Task<Teacher?> GetAsync(
        Expression<Func<Teacher, bool>> predicate,
        Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Teacher>?> GetListAsync(
        Expression<Func<Teacher, bool>>? predicate = null,
        Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>? orderBy = null,
        Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Teacher> AddAsync(Teacher teacher);
    Task<Teacher> UpdateAsync(Teacher teacher);
    Task<Teacher> DeleteAsync(Teacher teacher, bool permanent = false);
}
