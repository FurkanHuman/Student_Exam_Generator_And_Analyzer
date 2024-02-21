using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface IAnalysisHeaderRepository : IAsyncRepository<AnalysisHeader, int>, IRepository<AnalysisHeader, int>
{
}
