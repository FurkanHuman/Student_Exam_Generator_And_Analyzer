using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class OtpAuthenticatorRepository : EfRepositoryBase<OtpAuthenticator, Guid, PostgreSqlDbContext>, IOtpAuthenticatorRepository
{
    public OtpAuthenticatorRepository(PostgreSqlDbContext context)
        : base(context) { }
}
