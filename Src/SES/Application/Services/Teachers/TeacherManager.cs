using Application.Features.Teachers.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Teachers;

public class TeacherManager : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly TeacherBusinessRules _teacherBusinessRules;

    public TeacherManager(ITeacherRepository teacherRepository, TeacherBusinessRules teacherBusinessRules)
    {
        _teacherRepository = teacherRepository;
        _teacherBusinessRules = teacherBusinessRules;
    }

    public async Task<Teacher?> GetAsync(
        Expression<Func<Teacher, bool>> predicate,
        Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Teacher? teacher = await _teacherRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return teacher;
    }

    public async Task<IPaginate<Teacher>?> GetListAsync(
        Expression<Func<Teacher, bool>>? predicate = null,
        Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>? orderBy = null,
        Func<IQueryable<Teacher>, IIncludableQueryable<Teacher, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Teacher> teacherList = await _teacherRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return teacherList;
    }

    public async Task<Teacher> AddAsync(Teacher teacher)
    {
        Teacher addedTeacher = await _teacherRepository.AddAsync(teacher);

        return addedTeacher;
    }

    public async Task<Teacher> UpdateAsync(Teacher teacher)
    {
        Teacher updatedTeacher = await _teacherRepository.UpdateAsync(teacher);

        return updatedTeacher;
    }

    public async Task<Teacher> DeleteAsync(Teacher teacher, bool permanent = false)
    {
        Teacher deletedTeacher = await _teacherRepository.DeleteAsync(teacher);

        return deletedTeacher;
    }
}
