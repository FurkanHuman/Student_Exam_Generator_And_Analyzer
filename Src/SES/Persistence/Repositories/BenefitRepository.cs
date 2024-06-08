using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BenefitRepository : EfRepositoryBase<Benefit, int, PostgreSqlDbContext>, IBenefitRepository
{
    public BenefitRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}