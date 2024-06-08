using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IPrincipalRepository : IAsyncRepository<Principal, int>, IRepository<Principal, int>
{
}