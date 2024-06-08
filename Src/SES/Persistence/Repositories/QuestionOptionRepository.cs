using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class QuestionOptionRepository : EfRepositoryBase<QuestionOption, Guid, PostgreSqlDbContext>, IQuestionOptionRepository
{
    public QuestionOptionRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}