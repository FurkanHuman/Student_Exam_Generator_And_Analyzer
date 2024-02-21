
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class TeacherRepository : EfRepositoryBase<Teacher, int, PostgreSqlDbContext>, ITeacherRepository
{
    public TeacherRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

