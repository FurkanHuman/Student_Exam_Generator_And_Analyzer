using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IExamRepository : IAsyncRepository<Exam, int>, IRepository<Exam, int>
{
}
