using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface ITeacherRepository : IAsyncRepository<Teacher, int>, IRepository<Teacher, int>
{
}
