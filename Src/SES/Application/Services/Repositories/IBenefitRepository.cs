using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IBenefitRepository : IAsyncRepository<Benefit, int>, IRepository<Benefit, int>
{
}