using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class StudentAnswerRepository : EfRepositoryBase<StudentAnswer, Guid, PostgreSqlDbContext>, IStudentAnswerRepository
{
    public StudentAnswerRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}