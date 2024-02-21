
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class LearningAreaRepository : EfRepositoryBase<LearningArea, int, PostgreSqlDbContext>, ILearningAreaRepository
{
    public LearningAreaRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

