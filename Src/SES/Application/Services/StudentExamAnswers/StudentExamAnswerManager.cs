using Application.Features.StudentExamAnswers.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.StudentExamAnswers;

public class StudentExamAnswerManager : IStudentExamAnswerService
{
    private readonly IStudentExamAnswerRepository _studentExamAnswerRepository;
    private readonly StudentExamAnswerBusinessRules _studentExamAnswerBusinessRules;

    public StudentExamAnswerManager(IStudentExamAnswerRepository studentExamAnswerRepository, StudentExamAnswerBusinessRules studentExamAnswerBusinessRules)
    {
        _studentExamAnswerRepository = studentExamAnswerRepository;
        _studentExamAnswerBusinessRules = studentExamAnswerBusinessRules;
    }

    public async Task<StudentExamAnswer?> GetAsync(
        Expression<Func<StudentExamAnswer, bool>> predicate,
        Func<IQueryable<StudentExamAnswer>, IIncludableQueryable<StudentExamAnswer, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        StudentExamAnswer? studentExamAnswer = await _studentExamAnswerRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return studentExamAnswer;
    }

    public async Task<IPaginate<StudentExamAnswer>?> GetListAsync(
        Expression<Func<StudentExamAnswer, bool>>? predicate = null,
        Func<IQueryable<StudentExamAnswer>, IOrderedQueryable<StudentExamAnswer>>? orderBy = null,
        Func<IQueryable<StudentExamAnswer>, IIncludableQueryable<StudentExamAnswer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<StudentExamAnswer> studentExamAnswerList = await _studentExamAnswerRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return studentExamAnswerList;
    }

    public async Task<StudentExamAnswer> AddAsync(StudentExamAnswer studentExamAnswer)
    {
        StudentExamAnswer addedStudentExamAnswer = await _studentExamAnswerRepository.AddAsync(studentExamAnswer);

        return addedStudentExamAnswer;
    }

    public async Task<StudentExamAnswer> UpdateAsync(StudentExamAnswer studentExamAnswer)
    {
        StudentExamAnswer updatedStudentExamAnswer = await _studentExamAnswerRepository.UpdateAsync(studentExamAnswer);

        return updatedStudentExamAnswer;
    }

    public async Task<StudentExamAnswer> DeleteAsync(StudentExamAnswer studentExamAnswer, bool permanent = false)
    {
        StudentExamAnswer deletedStudentExamAnswer = await _studentExamAnswerRepository.DeleteAsync(studentExamAnswer);

        return deletedStudentExamAnswer;
    }
}
