using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class SemesterRepository : EfRepositoryBase<Semester, int, PostgreSqlDbContext>, ISemesterRepository
{
    public SemesterRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}