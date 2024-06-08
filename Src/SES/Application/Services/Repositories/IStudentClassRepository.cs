using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IStudentClassRepository : IAsyncRepository<StudentClass, int>, IRepository<StudentClass, int>
{
}