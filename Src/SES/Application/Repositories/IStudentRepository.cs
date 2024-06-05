using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IStudentRepository : IAsyncRepository<Student, int>, IRepository<Student, int>
{
}
