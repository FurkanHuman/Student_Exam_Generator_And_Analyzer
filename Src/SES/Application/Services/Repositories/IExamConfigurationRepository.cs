using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IExamConfigurationRepository : IAsyncRepository<ExamConfiguration, int>, IRepository<ExamConfiguration, int>
{
}