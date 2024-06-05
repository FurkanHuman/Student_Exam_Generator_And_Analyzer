using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IStudentAnswerRepository : IAsyncRepository<StudentAnswer, Guid>, IRepository<StudentAnswer, Guid>
{
}
