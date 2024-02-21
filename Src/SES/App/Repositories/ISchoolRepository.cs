using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface ISchoolRepository : IAsyncRepository<School, int>, IRepository<School, int>
{
}
