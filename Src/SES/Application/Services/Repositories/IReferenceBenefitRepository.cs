using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IReferenceBenefitRepository : IAsyncRepository<ReferenceBenefit, int>, IRepository<ReferenceBenefit, int>
{
}