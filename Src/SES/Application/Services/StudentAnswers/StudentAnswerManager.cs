using Application.Features.StudentAnswers.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.StudentAnswers;

public class StudentAnswerManager : IStudentAnswerService
{
    private readonly IStudentAnswerRepository _studentAnswerRepository;
    private readonly StudentAnswerBusinessRules _studentAnswerBusinessRules;

    public StudentAnswerManager(IStudentAnswerRepository studentAnswerRepository, StudentAnswerBusinessRules studentAnswerBusinessRules)
    {
        _studentAnswerRepository = studentAnswerRepository;
        _studentAnswerBusinessRules = studentAnswerBusinessRules;
    }

    public async Task<StudentAnswer?> GetAsync(
        Expression<Func<StudentAnswer, bool>> predicate,
        Func<IQueryable<StudentAnswer>, IIncludableQueryable<StudentAnswer, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        StudentAnswer? studentAnswer = await _studentAnswerRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return studentAnswer;
    }

    public async Task<IPaginate<StudentAnswer>?> GetListAsync(
        Expression<Func<StudentAnswer, bool>>? predicate = null,
        Func<IQueryable<StudentAnswer>, IOrderedQueryable<StudentAnswer>>? orderBy = null,
        Func<IQueryable<StudentAnswer>, IIncludableQueryable<StudentAnswer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<StudentAnswer> studentAnswerList = await _studentAnswerRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return studentAnswerList;
    }

    public async Task<StudentAnswer> AddAsync(StudentAnswer studentAnswer)
    {
        StudentAnswer addedStudentAnswer = await _studentAnswerRepository.AddAsync(studentAnswer);

        return addedStudentAnswer;
    }

    public async Task<StudentAnswer> UpdateAsync(StudentAnswer studentAnswer)
    {
        StudentAnswer updatedStudentAnswer = await _studentAnswerRepository.UpdateAsync(studentAnswer);

        return updatedStudentAnswer;
    }

    public async Task<StudentAnswer> DeleteAsync(StudentAnswer studentAnswer, bool permanent = false)
    {
        StudentAnswer deletedStudentAnswer = await _studentAnswerRepository.DeleteAsync(studentAnswer);

        return deletedStudentAnswer;
    }
}
