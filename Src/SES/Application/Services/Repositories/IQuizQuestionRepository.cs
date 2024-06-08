
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

internal interface IQuizQuestionRepository: IAsyncRepository<QuizQuestion,int>, IRepository<QuizQuestion,int>
{
}