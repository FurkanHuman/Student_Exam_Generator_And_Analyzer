using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class LearningAreaRepository : EfRepositoryBase<LearningArea, int, PostgreSqlDbContext>, ILearningAreaRepository
{
    public LearningAreaRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}