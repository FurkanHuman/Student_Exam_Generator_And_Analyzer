
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BenefitRepository : EfRepositoryBase<Benefit, int, PostgreSqlDbContext>, IBenefitRepository
{
    public BenefitRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

