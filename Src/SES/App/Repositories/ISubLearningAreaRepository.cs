using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface ISubLearningAreaRepository : IAsyncRepository<SubLearningArea, int>, IRepository<SubLearningArea, int>
{
}
