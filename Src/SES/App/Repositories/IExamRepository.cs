using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface IExamRepository : IAsyncRepository<Exam, int>, IRepository<Exam, int>
{
}
