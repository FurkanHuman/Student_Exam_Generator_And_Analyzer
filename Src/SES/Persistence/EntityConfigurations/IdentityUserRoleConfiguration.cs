using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        //builder.HasData(GetSeedUserRole());
    }

    private static IdentityUserRole<Guid> GetSeedUserRole()
    {
        return new IdentityUserRole<Guid>
        {
            UserId = ApplicationUserConfiguration.AdminGuid,
            RoleId = IdentityRoleConfiguration.AdminRoleGuid
        };
    }
}
