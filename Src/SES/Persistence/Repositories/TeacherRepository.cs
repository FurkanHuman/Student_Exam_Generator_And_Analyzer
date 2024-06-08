using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class TeacherRepository : EfRepositoryBase<Teacher, int, PostgreSqlDbContext>, ITeacherRepository
{
    public TeacherRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}