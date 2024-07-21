using Application.Features.StudentClasses.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.StudentClasses;

public class StudentClassManager : IStudentClassService
{
    private readonly IStudentClassRepository _studentClassRepository;
    private readonly StudentClassBusinessRules _studentClassBusinessRules;

    public StudentClassManager(IStudentClassRepository studentClassRepository, StudentClassBusinessRules studentClassBusinessRules)
    {
        _studentClassRepository = studentClassRepository;
        _studentClassBusinessRules = studentClassBusinessRules;
    }

    public async Task<StudentClass?> GetAsync(
        Expression<Func<StudentClass, bool>> predicate,
        Func<IQueryable<StudentClass>, IIncludableQueryable<StudentClass, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        StudentClass? studentClass = await _studentClassRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return studentClass;
    }

    public async Task<IPaginate<StudentClass>?> GetListAsync(
        Expression<Func<StudentClass, bool>>? predicate = null,
        Func<IQueryable<StudentClass>, IOrderedQueryable<StudentClass>>? orderBy = null,
        Func<IQueryable<StudentClass>, IIncludableQueryable<StudentClass, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<StudentClass> studentClassList = await _studentClassRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return studentClassList;
    }

    public async Task<StudentClass> AddAsync(StudentClass studentClass)
    {
        StudentClass addedStudentClass = await _studentClassRepository.AddAsync(studentClass);

        return addedStudentClass;
    }

    public async Task<StudentClass> UpdateAsync(StudentClass studentClass)
    {
        StudentClass updatedStudentClass = await _studentClassRepository.UpdateAsync(studentClass);

        return updatedStudentClass;
    }

    public async Task<StudentClass> DeleteAsync(StudentClass studentClass, bool permanent = false)
    {
        StudentClass deletedStudentClass = await _studentClassRepository.DeleteAsync(studentClass);

        return deletedStudentClass;
    }
}
