
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class QuestionScoreRepository : EfRepositoryBase<QuestionScore, int, PostgreSqlDbContext>, IQuestionScoreRepository
{
    public QuestionScoreRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}

