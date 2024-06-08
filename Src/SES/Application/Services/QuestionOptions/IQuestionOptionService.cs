using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.QuestionOptions;

public interface IQuestionOptionService
{
    Task<QuestionOption?> GetAsync(
        Expression<Func<QuestionOption, bool>> predicate,
        Func<IQueryable<QuestionOption>, IIncludableQueryable<QuestionOption, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<QuestionOption>?> GetListAsync(
        Expression<Func<QuestionOption, bool>>? predicate = null,
        Func<IQueryable<QuestionOption>, IOrderedQueryable<QuestionOption>>? orderBy = null,
        Func<IQueryable<QuestionOption>, IIncludableQueryable<QuestionOption, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<QuestionOption> AddAsync(QuestionOption questionOption);
    Task<QuestionOption> UpdateAsync(QuestionOption questionOption);
    Task<QuestionOption> DeleteAsync(QuestionOption questionOption, bool permanent = false);
}
