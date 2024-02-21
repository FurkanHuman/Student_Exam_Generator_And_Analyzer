using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Contexts;

public class PostgreSqlDbContext : BaseDbContext
{
    public PostgreSqlDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions, configuration)
    {
    }
}
