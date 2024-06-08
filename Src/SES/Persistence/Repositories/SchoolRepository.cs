using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class SchoolRepository : EfRepositoryBase<School, int, PostgreSqlDbContext>, ISchoolRepository
{
    public SchoolRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}