using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ExamRepository : EfRepositoryBase<Exam, int, PostgreSqlDbContext>, IExamRepository
{
    public ExamRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}