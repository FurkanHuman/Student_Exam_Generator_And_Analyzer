using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.StudentAnswers;

public interface IStudentAnswerService
{
    Task<StudentAnswer?> GetAsync(
        Expression<Func<StudentAnswer, bool>> predicate,
        Func<IQueryable<StudentAnswer>, IIncludableQueryable<StudentAnswer, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<StudentAnswer>?> GetListAsync(
        Expression<Func<StudentAnswer, bool>>? predicate = null,
        Func<IQueryable<StudentAnswer>, IOrderedQueryable<StudentAnswer>>? orderBy = null,
        Func<IQueryable<StudentAnswer>, IIncludableQueryable<StudentAnswer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<StudentAnswer> AddAsync(StudentAnswer studentAnswer);
    Task<StudentAnswer> UpdateAsync(StudentAnswer studentAnswer);
    Task<StudentAnswer> DeleteAsync(StudentAnswer studentAnswer, bool permanent = false);
}
