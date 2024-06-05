using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface ISubLearningAreaRepository : IAsyncRepository<SubLearningArea, int>, IRepository<SubLearningArea, int>
{
}
