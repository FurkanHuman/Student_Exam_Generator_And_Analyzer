using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class SubLearningAreaRepository : EfRepositoryBase<SubLearningArea, int, PostgreSqlDbContext>, ISubLearningAreaRepository
{
    public SubLearningAreaRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}