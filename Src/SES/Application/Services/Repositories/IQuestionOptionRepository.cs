using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IQuestionOptionRepository : IAsyncRepository<QuestionOption, Guid>, IRepository<QuestionOption, Guid>
{
}