using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>, IUserConfiguration
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}
