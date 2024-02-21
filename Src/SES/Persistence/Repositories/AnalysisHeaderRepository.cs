
using App.Repositories;
using Entity.Entities.Mains;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

//public class EmailAuthenticatorRepository : EfRepositoryBase<EmailAuthenticator, int, BaseDbContext>, IEmailAuthenticatorRepository
//{
//    public EmailAuthenticatorRepository(BaseDbContext context)
//        : base(context) { }
//}

public class AnalysisHeaderRepository : EfRepositoryBase<AnalysisHeader, int, PostgreSqlDbContext>, IAnalysisHeaderRepository
{
    public AnalysisHeaderRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
