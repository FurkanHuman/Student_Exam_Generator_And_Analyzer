using Application.Features.Semesters.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Semesters;

public class SemesterManager : ISemesterService
{
    private readonly ISemesterRepository _semesterRepository;
    private readonly SemesterBusinessRules _semesterBusinessRules;

    public SemesterManager(ISemesterRepository semesterRepository, SemesterBusinessRules semesterBusinessRules)
    {
        _semesterRepository = semesterRepository;
        _semesterBusinessRules = semesterBusinessRules;
    }

    public async Task<Semester?> GetAsync(
        Expression<Func<Semester, bool>> predicate,
        Func<IQueryable<Semester>, IIncludableQueryable<Semester, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Semester? semester = await _semesterRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return semester;
    }

    public async Task<IPaginate<Semester>?> GetListAsync(
        Expression<Func<Semester, bool>>? predicate = null,
        Func<IQueryable<Semester>, IOrderedQueryable<Semester>>? orderBy = null,
        Func<IQueryable<Semester>, IIncludableQueryable<Semester, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Semester> semesterList = await _semesterRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return semesterList;
    }

    public async Task<Semester> AddAsync(Semester semester)
    {
        Semester addedSemester = await _semesterRepository.AddAsync(semester);

        return addedSemester;
    }

    public async Task<Semester> UpdateAsync(Semester semester)
    {
        Semester updatedSemester = await _semesterRepository.UpdateAsync(semester);

        return updatedSemester;
    }

    public async Task<Semester> DeleteAsync(Semester semester, bool permanent = false)
    {
        Semester deletedSemester = await _semesterRepository.DeleteAsync(semester);

        return deletedSemester;
    }
}
