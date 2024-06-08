using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface ISchoolRepository : IAsyncRepository<School, int>, IRepository<School, int>
{
}