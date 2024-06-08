using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ReferenceBenefitRepository : EfRepositoryBase<ReferenceBenefit, int, PostgreSqlDbContext>, IReferenceBenefitRepository
{
    public ReferenceBenefitRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}