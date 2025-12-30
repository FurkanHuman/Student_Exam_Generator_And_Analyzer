using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.ExamConfigurations;

public interface IExamConfigurationService
{
    Task<ExamConfiguration?> GetAsync(
        Expression<Func<ExamConfiguration, bool>> predicate,
        Func<IQueryable<ExamConfiguration>, IIncludableQueryable<ExamConfiguration, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<ExamConfiguration>?> GetListAsync(
        Expression<Func<ExamConfiguration, bool>>? predicate = null,
        Func<IQueryable<ExamConfiguration>, IOrderedQueryable<ExamConfiguration>>? orderBy = null,
        Func<IQueryable<ExamConfiguration>, IIncludableQueryable<ExamConfiguration, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<ExamConfiguration> AddAsync(ExamConfiguration examConfiguration);
    Task<ExamConfiguration> UpdateAsync(ExamConfiguration examConfiguration);
    Task<ExamConfiguration> DeleteAsync(ExamConfiguration examConfiguration, bool permanent = false);
}
