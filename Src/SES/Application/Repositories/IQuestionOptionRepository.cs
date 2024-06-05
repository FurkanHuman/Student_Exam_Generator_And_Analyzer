using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Repositories;

public interface IQuestionOptionRepository : IAsyncRepository<QuestionOption, Guid>, IRepository<QuestionOption, Guid>
{
}
