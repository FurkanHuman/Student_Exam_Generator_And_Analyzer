using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface ISubLearningAreaRepository : IAsyncRepository<SubLearningArea, int>, IRepository<SubLearningArea, int>
{
}