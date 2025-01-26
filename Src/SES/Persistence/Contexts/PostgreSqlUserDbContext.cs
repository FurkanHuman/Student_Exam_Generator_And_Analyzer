using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.Contexts
{
    public class PostgreSqlUserDbContext(DbContextOptions<PostgreSqlUserDbContext> options, IConfiguration configuration) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {
        protected IConfiguration? Configuration { get; set; } = configuration;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsWithInterface<IUserConfiguration>();

            builder.HasDefaultSchema("SES_Microsoft.AspNetCore.Identity.EntityFrameworkCore");
            base.OnModelCreating(builder);
        }

    }
}
