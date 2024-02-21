using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface IReferenceBenefitRepository : IAsyncRepository<ReferenceBenefit, int>, IRepository<ReferenceBenefit, int>
{
}
