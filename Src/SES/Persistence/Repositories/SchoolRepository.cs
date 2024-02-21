
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class SchoolRepository : EfRepositoryBase<School, int, PostgreSqlDbContext>, ISchoolRepository
{
    public SchoolRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

