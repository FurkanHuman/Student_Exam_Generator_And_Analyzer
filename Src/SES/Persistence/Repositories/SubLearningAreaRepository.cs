
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class SubLearningAreaRepository : EfRepositoryBase<SubLearningArea, int, PostgreSqlDbContext>, ISubLearningAreaRepository
{
    public SubLearningAreaRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

