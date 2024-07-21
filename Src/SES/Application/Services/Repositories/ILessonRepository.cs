using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface ILessonRepository : IAsyncRepository<Lesson, int>, IRepository<Lesson, int>
{
}