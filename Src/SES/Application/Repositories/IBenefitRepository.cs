using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IBenefitRepository : IAsyncRepository<Benefit, int>, IRepository<Benefit, int>
{
}
