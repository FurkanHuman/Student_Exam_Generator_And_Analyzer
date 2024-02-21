
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ReferenceBenefitRepository : EfRepositoryBase<ReferenceBenefit, int, PostgreSqlDbContext>, IReferenceBenefitRepository
{
    public ReferenceBenefitRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

