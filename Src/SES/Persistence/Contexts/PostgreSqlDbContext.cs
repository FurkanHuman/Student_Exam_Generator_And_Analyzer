using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.Contexts;

public class PostgreSqlDbContext : BaseDbContext
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions, configuration) => Configuration = configuration;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsWithInterface<IMainConfiguration>();

        modelBuilder.HasDefaultSchema("SES_PostgreSql_Main");
        base.OnModelCreating(modelBuilder);
    }
}
