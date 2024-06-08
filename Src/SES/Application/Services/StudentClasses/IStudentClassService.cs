using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.StudentClasses;

public interface IStudentClassService
{
    Task<StudentClass?> GetAsync(
        Expression<Func<StudentClass, bool>> predicate,
        Func<IQueryable<StudentClass>, IIncludableQueryable<StudentClass, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<StudentClass>?> GetListAsync(
        Expression<Func<StudentClass, bool>>? predicate = null,
        Func<IQueryable<StudentClass>, IOrderedQueryable<StudentClass>>? orderBy = null,
        Func<IQueryable<StudentClass>, IIncludableQueryable<StudentClass, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<StudentClass> AddAsync(StudentClass studentClass);
    Task<StudentClass> UpdateAsync(StudentClass studentClass);
    Task<StudentClass> DeleteAsync(StudentClass studentClass, bool permanent = false);
}
