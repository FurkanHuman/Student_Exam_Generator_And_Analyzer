using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>, IUserConfiguration
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {

    }
}
