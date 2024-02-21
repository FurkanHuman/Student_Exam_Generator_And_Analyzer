using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface IStudentRepository : IAsyncRepository<Student, int>, IRepository<Student, int>
{
}
