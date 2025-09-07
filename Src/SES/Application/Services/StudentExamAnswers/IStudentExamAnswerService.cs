using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.StudentExamAnswers;

public interface IStudentExamAnswerService
{
    Task<StudentExamAnswer?> GetAsync(
        Expression<Func<StudentExamAnswer, bool>> predicate,
        Func<IQueryable<StudentExamAnswer>, IIncludableQueryable<StudentExamAnswer, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<StudentExamAnswer>?> GetListAsync(
        Expression<Func<StudentExamAnswer, bool>>? predicate = null,
        Func<IQueryable<StudentExamAnswer>, IOrderedQueryable<StudentExamAnswer>>? orderBy = null,
        Func<IQueryable<StudentExamAnswer>, IIncludableQueryable<StudentExamAnswer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<StudentExamAnswer> AddAsync(StudentExamAnswer studentExamAnswer);
    Task<StudentExamAnswer> UpdateAsync(StudentExamAnswer studentExamAnswer);
    Task<StudentExamAnswer> DeleteAsync(StudentExamAnswer studentExamAnswer, bool permanent = false);
}
