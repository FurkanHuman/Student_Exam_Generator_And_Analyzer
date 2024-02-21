
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class QuizForAnswersRepository : EfRepositoryBase<QuizForAnswers, int, PostgreSqlDbContext>, IQuizForAnswersRepository
{
    public QuizForAnswersRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

