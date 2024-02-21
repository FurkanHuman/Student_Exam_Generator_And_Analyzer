using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace App.Repositories;

public interface IQuizForAnswersRepository : IAsyncRepository<QuizForAnswers, int>, IRepository<QuizForAnswers, int>
{
}
