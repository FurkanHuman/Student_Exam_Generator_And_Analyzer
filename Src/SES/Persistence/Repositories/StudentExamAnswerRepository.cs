using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class StudentExamAnswerRepository : EfRepositoryBase<StudentExamAnswer, Guid, PostgreSqlDbContext>, IStudentExamAnswerRepository
{
    public StudentExamAnswerRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}