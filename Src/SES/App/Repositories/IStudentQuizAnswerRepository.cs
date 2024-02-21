using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface IStudentQuizAnswerRepository : IAsyncRepository<StudentQuizAnswer, int>, IRepository<StudentQuizAnswer, int>
{
}
