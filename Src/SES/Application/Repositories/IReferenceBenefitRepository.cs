using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IReferenceBenefitRepository : IAsyncRepository<ReferenceBenefit, int>, IRepository<ReferenceBenefit, int>
{
}
