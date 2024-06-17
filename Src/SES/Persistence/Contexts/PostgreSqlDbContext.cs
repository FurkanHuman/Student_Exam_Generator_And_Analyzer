using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Persistence.Contexts;

public class PostgreSqlDbContext : BaseDbContext
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions,configuration) => Configuration = configuration;

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
