using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UserOperationClaimRepository
    : EfRepositoryBase<UserOperationClaim, Guid, PostgreSqlDbContext>,
        IUserOperationClaimRepository
{
    public UserOperationClaimRepository(PostgreSqlDbContext context)
        : base(context) { }

    public async Task<IList<OperationClaim>> GetOperationClaimsByUserIdAsync(Guid userId)
    {
        List<OperationClaim> operationClaims = await Query()
            .AsNoTracking()
            .Where(p => p.UserId.Equals(userId))
            .Select(p => new OperationClaim { Id = p.OperationClaimId, Name = p.OperationClaim.Name })
            .ToListAsync();
        return operationClaims;
    }
}
