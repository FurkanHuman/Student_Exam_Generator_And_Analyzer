using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ExamConfigurationRepository : EfRepositoryBase<ExamConfiguration, int, PostgreSqlDbContext>, IExamConfigurationRepository
{
    public ExamConfigurationRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}