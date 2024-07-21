using Application.Features.QuestionScores.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.QuestionScores;

public class QuestionScoreManager : IQuestionScoreService
{
    private readonly IQuestionScoreRepository _questionScoreRepository;
    private readonly QuestionScoreBusinessRules _questionScoreBusinessRules;

    public QuestionScoreManager(IQuestionScoreRepository questionScoreRepository, QuestionScoreBusinessRules questionScoreBusinessRules)
    {
        _questionScoreRepository = questionScoreRepository;
        _questionScoreBusinessRules = questionScoreBusinessRules;
    }

    public async Task<QuestionScore?> GetAsync(
        Expression<Func<QuestionScore, bool>> predicate,
        Func<IQueryable<QuestionScore>, IIncludableQueryable<QuestionScore, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        QuestionScore? questionScore = await _questionScoreRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return questionScore;
    }

    public async Task<IPaginate<QuestionScore>?> GetListAsync(
        Expression<Func<QuestionScore, bool>>? predicate = null,
        Func<IQueryable<QuestionScore>, IOrderedQueryable<QuestionScore>>? orderBy = null,
        Func<IQueryable<QuestionScore>, IIncludableQueryable<QuestionScore, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<QuestionScore> questionScoreList = await _questionScoreRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return questionScoreList;
    }

    public async Task<QuestionScore> AddAsync(QuestionScore questionScore)
    {
        QuestionScore addedQuestionScore = await _questionScoreRepository.AddAsync(questionScore);

        return addedQuestionScore;
    }

    public async Task<QuestionScore> UpdateAsync(QuestionScore questionScore)
    {
        QuestionScore updatedQuestionScore = await _questionScoreRepository.UpdateAsync(questionScore);

        return updatedQuestionScore;
    }

    public async Task<QuestionScore> DeleteAsync(QuestionScore questionScore, bool permanent = false)
    {
        QuestionScore deletedQuestionScore = await _questionScoreRepository.DeleteAsync(questionScore);

        return deletedQuestionScore;
    }
}
