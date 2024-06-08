using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class QuestionScoreRepository : EfRepositoryBase<QuestionScore, int, PostgreSqlDbContext>, IQuestionScoreRepository
{
    public QuestionScoreRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}