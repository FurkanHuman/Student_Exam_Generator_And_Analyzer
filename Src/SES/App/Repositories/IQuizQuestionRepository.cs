using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface IQuizQuestionRepository : IAsyncRepository<QuizQuestion, int>, IRepository<QuizQuestion, int>
{
}
