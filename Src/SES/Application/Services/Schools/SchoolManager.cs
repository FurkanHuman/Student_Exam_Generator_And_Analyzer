using Application.Features.Schools.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Schools;

public class SchoolManager : ISchoolService
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly SchoolBusinessRules _schoolBusinessRules;

    public SchoolManager(ISchoolRepository schoolRepository, SchoolBusinessRules schoolBusinessRules)
    {
        _schoolRepository = schoolRepository;
        _schoolBusinessRules = schoolBusinessRules;
    }

    public async Task<School?> GetAsync(
        Expression<Func<School, bool>> predicate,
        Func<IQueryable<School>, IIncludableQueryable<School, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        School? school = await _schoolRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return school;
    }

    public async Task<IPaginate<School>?> GetListAsync(
        Expression<Func<School, bool>>? predicate = null,
        Func<IQueryable<School>, IOrderedQueryable<School>>? orderBy = null,
        Func<IQueryable<School>, IIncludableQueryable<School, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<School> schoolList = await _schoolRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return schoolList;
    }

    public async Task<School> AddAsync(School school)
    {
        School addedSchool = await _schoolRepository.AddAsync(school);

        return addedSchool;
    }

    public async Task<School> UpdateAsync(School school)
    {
        School updatedSchool = await _schoolRepository.UpdateAsync(school);

        return updatedSchool;
    }

    public async Task<School> DeleteAsync(School school, bool permanent = false)
    {
        School deletedSchool = await _schoolRepository.DeleteAsync(school);

        return deletedSchool;
    }
}
