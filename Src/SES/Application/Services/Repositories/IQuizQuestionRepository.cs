using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IQuizQuestionRepository : IAsyncRepository<QuizQuestion, int>, IRepository<QuizQuestion, int>
{
}