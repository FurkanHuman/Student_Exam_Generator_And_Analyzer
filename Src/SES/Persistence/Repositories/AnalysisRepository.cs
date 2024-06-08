using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class AnalysisRepository : EfRepositoryBase<Analysis, int, PostgreSqlDbContext>, IAnalysisRepository
{
    public AnalysisRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}