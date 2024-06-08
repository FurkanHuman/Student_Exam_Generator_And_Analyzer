using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class StudentRepository : EfRepositoryBase<Student, int, PostgreSqlDbContext>, IStudentRepository
{
    public StudentRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}