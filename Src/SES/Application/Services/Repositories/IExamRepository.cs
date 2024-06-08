using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IExamRepository : IAsyncRepository<Exam, int>, IRepository<Exam, int>
{
}