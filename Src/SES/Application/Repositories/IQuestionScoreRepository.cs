using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IQuestionScoreRepository : IAsyncRepository<QuestionScore, int>, IRepository<QuestionScore, int>
{
}
