using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IPrincipalRepository : IAsyncRepository<Principal, int>, IRepository<Principal, int>
{
}
