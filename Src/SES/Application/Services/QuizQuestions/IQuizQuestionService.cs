using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.QuizQuestions;

public interface IQuizQuestionService
{
    Task<QuizQuestion?> GetAsync(
        Expression<Func<QuizQuestion, bool>> predicate,
        Func<IQueryable<QuizQuestion>, IIncludableQueryable<QuizQuestion, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<QuizQuestion>?> GetListAsync(
        Expression<Func<QuizQuestion, bool>>? predicate = null,
        Func<IQueryable<QuizQuestion>, IOrderedQueryable<QuizQuestion>>? orderBy = null,
        Func<IQueryable<QuizQuestion>, IIncludableQueryable<QuizQuestion, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<QuizQuestion> AddAsync(QuizQuestion quizQuestion);
    Task<QuizQuestion> UpdateAsync(QuizQuestion quizQuestion);
    Task<QuizQuestion> DeleteAsync(QuizQuestion quizQuestion, bool permanent = false);
}
