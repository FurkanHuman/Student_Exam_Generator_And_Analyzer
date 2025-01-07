using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Contexts;

public class PostgreSqlDbContext : BaseDbContext
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions, configuration) => Configuration = configuration;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SES_PostgreSql_Main");
        base.OnModelCreating(modelBuilder);
    }
}
