using Application.Features.QuestionOptions.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.QuestionOptions;

public class QuestionOptionManager : IQuestionOptionService
{
    private readonly IQuestionOptionRepository _questionOptionRepository;
    private readonly QuestionOptionBusinessRules _questionOptionBusinessRules;

    public QuestionOptionManager(IQuestionOptionRepository questionOptionRepository, QuestionOptionBusinessRules questionOptionBusinessRules)
    {
        _questionOptionRepository = questionOptionRepository;
        _questionOptionBusinessRules = questionOptionBusinessRules;
    }

    public async Task<QuestionOption?> GetAsync(
        Expression<Func<QuestionOption, bool>> predicate,
        Func<IQueryable<QuestionOption>, IIncludableQueryable<QuestionOption, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        QuestionOption? questionOption = await _questionOptionRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return questionOption;
    }

    public async Task<IPaginate<QuestionOption>?> GetListAsync(
        Expression<Func<QuestionOption, bool>>? predicate = null,
        Func<IQueryable<QuestionOption>, IOrderedQueryable<QuestionOption>>? orderBy = null,
        Func<IQueryable<QuestionOption>, IIncludableQueryable<QuestionOption, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<QuestionOption> questionOptionList = await _questionOptionRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return questionOptionList;
    }

    public async Task<QuestionOption> AddAsync(QuestionOption questionOption)
    {
        QuestionOption addedQuestionOption = await _questionOptionRepository.AddAsync(questionOption);

        return addedQuestionOption;
    }

    public async Task<QuestionOption> UpdateAsync(QuestionOption questionOption)
    {
        QuestionOption updatedQuestionOption = await _questionOptionRepository.UpdateAsync(questionOption);

        return updatedQuestionOption;
    }

    public async Task<QuestionOption> DeleteAsync(QuestionOption questionOption, bool permanent = false)
    {
        QuestionOption deletedQuestionOption = await _questionOptionRepository.DeleteAsync(questionOption);

        return deletedQuestionOption;
    }
}
