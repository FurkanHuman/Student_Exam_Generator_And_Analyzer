
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class PrincipalRepository : EfRepositoryBase<Principal, int, PostgreSqlDbContext>, IPrincipalRepository
{
    public PrincipalRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

