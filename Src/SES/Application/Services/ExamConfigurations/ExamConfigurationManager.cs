using Application.Features.ExamConfigurations.Rules;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using NArchitecture.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Application.Services.ExamConfigurations;

public class ExamConfigurationManager : IExamConfigurationService
{
    private readonly IExamConfigurationRepository _examConfigurationRepository;
    private readonly ExamConfigurationBusinessRules _examConfigurationBusinessRules;

    public ExamConfigurationManager(IExamConfigurationRepository examConfigurationRepository, ExamConfigurationBusinessRules examConfigurationBusinessRules)
    {
        _examConfigurationRepository = examConfigurationRepository;
        _examConfigurationBusinessRules = examConfigurationBusinessRules;
    }

    public async Task<ExamConfiguration?> GetAsync(
        Expression<Func<ExamConfiguration, bool>> predicate,
        Func<IQueryable<ExamConfiguration>, IIncludableQueryable<ExamConfiguration, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        ExamConfiguration? examConfiguration = await _examConfigurationRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return examConfiguration;
    }

    public async Task<IPaginate<ExamConfiguration>?> GetListAsync(
        Expression<Func<ExamConfiguration, bool>>? predicate = null,
        Func<IQueryable<ExamConfiguration>, IOrderedQueryable<ExamConfiguration>>? orderBy = null,
        Func<IQueryable<ExamConfiguration>, IIncludableQueryable<ExamConfiguration, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<ExamConfiguration> examConfigurationList = await _examConfigurationRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return examConfigurationList;
    }

    public async Task<ExamConfiguration> AddAsync(ExamConfiguration examConfiguration)
    {
        ExamConfiguration addedExamConfiguration = await _examConfigurationRepository.AddAsync(examConfiguration);

        return addedExamConfiguration;
    }

    public async Task<ExamConfiguration> UpdateAsync(ExamConfiguration examConfiguration)
    {
        ExamConfiguration updatedExamConfiguration = await _examConfigurationRepository.UpdateAsync(examConfiguration);

        return updatedExamConfiguration;
    }

    public async Task<ExamConfiguration> DeleteAsync(ExamConfiguration examConfiguration, bool permanent = false)
    {
        ExamConfiguration deletedExamConfiguration = await _examConfigurationRepository.DeleteAsync(examConfiguration);

        return deletedExamConfiguration;
    }
}
