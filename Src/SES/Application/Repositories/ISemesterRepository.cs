using Entity.Entities.Infos;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface ISemesterRepository : IAsyncRepository<Semester, int>, IRepository<Semester, int>
{
}

