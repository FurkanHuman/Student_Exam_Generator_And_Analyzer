using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IFeedBackRepository : IAsyncRepository<FeedBack, Guid>, IRepository<FeedBack, Guid>
{
}