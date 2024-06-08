using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IAnalysisRepository : IAsyncRepository<Analysis, int>, IRepository<Analysis, int>
{
}