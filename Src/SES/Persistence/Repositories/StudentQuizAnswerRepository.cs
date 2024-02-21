
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class StudentQuizAnswerRepository : EfRepositoryBase<StudentQuizAnswer, int, PostgreSqlDbContext>, IStudentQuizAnswerRepository
{
    public StudentQuizAnswerRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

