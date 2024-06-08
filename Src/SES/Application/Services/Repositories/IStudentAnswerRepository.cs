using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IStudentAnswerRepository : IAsyncRepository<StudentAnswer, Guid>, IRepository<StudentAnswer, Guid>
{
}