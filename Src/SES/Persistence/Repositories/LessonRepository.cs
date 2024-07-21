using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class LessonRepository : EfRepositoryBase<Lesson, int, PostgreSqlDbContext>, ILessonRepository
{
    public LessonRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}