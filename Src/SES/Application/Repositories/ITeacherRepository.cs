using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface ITeacherRepository : IAsyncRepository<Teacher, int>, IRepository<Teacher, int>
{
}
