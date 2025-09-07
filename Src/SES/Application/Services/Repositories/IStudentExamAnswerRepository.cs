using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IStudentExamAnswerRepository : IAsyncRepository<StudentExamAnswer, Guid>, IRepository<StudentExamAnswer, Guid>
{
}