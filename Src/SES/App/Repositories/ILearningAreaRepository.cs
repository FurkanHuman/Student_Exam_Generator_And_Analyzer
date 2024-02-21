using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface ILearningAreaRepository : IAsyncRepository<LearningArea, int>, IRepository<LearningArea, int>
{
}
