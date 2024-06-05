using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface ILearningAreaRepository : IAsyncRepository<LearningArea, int>, IRepository<LearningArea, int>
{
}
