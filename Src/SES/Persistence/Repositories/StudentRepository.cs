
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class StudentRepository : EfRepositoryBase<Student, int, PostgreSqlDbContext>, IStudentRepository
{
    public StudentRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

