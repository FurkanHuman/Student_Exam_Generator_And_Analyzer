using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IAnalysisRepository : IAsyncRepository<Analysis, int>, IRepository<Analysis, int>
{
}

