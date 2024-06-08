using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.QuestionScores;

public interface IQuestionScoreService
{
    Task<QuestionScore?> GetAsync(
        Expression<Func<QuestionScore, bool>> predicate,
        Func<IQueryable<QuestionScore>, IIncludableQueryable<QuestionScore, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<QuestionScore>?> GetListAsync(
        Expression<Func<QuestionScore, bool>>? predicate = null,
        Func<IQueryable<QuestionScore>, IOrderedQueryable<QuestionScore>>? orderBy = null,
        Func<IQueryable<QuestionScore>, IIncludableQueryable<QuestionScore, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<QuestionScore> AddAsync(QuestionScore questionScore);
    Task<QuestionScore> UpdateAsync(QuestionScore questionScore);
    Task<QuestionScore> DeleteAsync(QuestionScore questionScore, bool permanent = false);
}
