using Application.Features.QuizQuestions.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.QuizQuestions;

public class QuizQuestionManager : IQuizQuestionService
{
    private readonly IQuizQuestionRepository _quizQuestionRepository;
    private readonly QuizQuestionBusinessRules _quizQuestionBusinessRules;

    public QuizQuestionManager(IQuizQuestionRepository quizQuestionRepository, QuizQuestionBusinessRules quizQuestionBusinessRules)
    {
        _quizQuestionRepository = quizQuestionRepository;
        _quizQuestionBusinessRules = quizQuestionBusinessRules;
    }

    public async Task<QuizQuestion?> GetAsync(
        Expression<Func<QuizQuestion, bool>> predicate,
        Func<IQueryable<QuizQuestion>, IIncludableQueryable<QuizQuestion, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        QuizQuestion? quizQuestion = await _quizQuestionRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return quizQuestion;
    }

    public async Task<IPaginate<QuizQuestion>?> GetListAsync(
        Expression<Func<QuizQuestion, bool>>? predicate = null,
        Func<IQueryable<QuizQuestion>, IOrderedQueryable<QuizQuestion>>? orderBy = null,
        Func<IQueryable<QuizQuestion>, IIncludableQueryable<QuizQuestion, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<QuizQuestion> quizQuestionList = await _quizQuestionRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return quizQuestionList;
    }

    public async Task<QuizQuestion> AddAsync(QuizQuestion quizQuestion)
    {
        QuizQuestion addedQuizQuestion = await _quizQuestionRepository.AddAsync(quizQuestion);

        return addedQuizQuestion;
    }

    public async Task<QuizQuestion> UpdateAsync(QuizQuestion quizQuestion)
    {
        QuizQuestion updatedQuizQuestion = await _quizQuestionRepository.UpdateAsync(quizQuestion);

        return updatedQuizQuestion;
    }

    public async Task<QuizQuestion> DeleteAsync(QuizQuestion quizQuestion, bool permanent = false)
    {
        QuizQuestion deletedQuizQuestion = await _quizQuestionRepository.DeleteAsync(quizQuestion);

        return deletedQuizQuestion;
    }
}
