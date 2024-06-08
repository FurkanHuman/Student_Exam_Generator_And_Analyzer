using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IQuestionScoreRepository : IAsyncRepository<QuestionScore, int>, IRepository<QuestionScore, int>
{
}