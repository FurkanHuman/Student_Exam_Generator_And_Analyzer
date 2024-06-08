using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Domain.Entities;

namespace Persistence.Contexts;

public class PostgreSqlDbContext : BaseDbContext
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions, configuration)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
