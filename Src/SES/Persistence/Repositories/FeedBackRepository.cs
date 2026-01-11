using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class FeedBackRepository : EfRepositoryBase<FeedBack, Guid, PostgreSqlDbContext>, IFeedBackRepository
{
    public FeedBackRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}