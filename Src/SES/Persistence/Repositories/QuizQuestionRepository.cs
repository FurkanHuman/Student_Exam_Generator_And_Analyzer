
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class QuizQuestionRepository : EfRepositoryBase<QuizQuestion, int, PostgreSqlDbContext>, IQuizQuestionRepository
{
    public QuizQuestionRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

