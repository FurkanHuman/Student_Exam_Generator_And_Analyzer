using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class StudentClassRepository : EfRepositoryBase<StudentClass, int, PostgreSqlDbContext>, IStudentClassRepository
{
    public StudentClassRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}