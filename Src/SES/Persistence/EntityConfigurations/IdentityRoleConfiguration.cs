using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>, IUserConfiguration
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.HasData(getSeedRoles());
    }

    internal static Guid AdminRoleGuid { get; set; } = Guid.NewGuid();

    private IEnumerable<IdentityRole<Guid>> getSeedRoles() => new IdentityRole<Guid>[]
    {
        new() { Id = AdminRoleGuid, Name = RoleTypeConstants.Admin, NormalizedName = RoleTypeConstants.Admin.ToUpper()},
        new() { Id = Guid.NewGuid(), Name = RoleTypeConstants.Authorized, NormalizedName = RoleTypeConstants.Authorized.ToUpper() },
        new() { Id = Guid.NewGuid(), Name = RoleTypeConstants.Unauthorized, NormalizedName = RoleTypeConstants.Unauthorized.ToUpper()},
        new() { Id = Guid.NewGuid(), Name = RoleTypeConstants.Director, NormalizedName = RoleTypeConstants.Director.ToUpper() },
        new() { Id = Guid.NewGuid(), Name = RoleTypeConstants.AssistantDirector, NormalizedName = RoleTypeConstants.AssistantDirector.ToUpper() },
        new() { Id = Guid.NewGuid(), Name = RoleTypeConstants.HeadTeacher, NormalizedName = RoleTypeConstants.HeadTeacher.ToUpper() },
        new() { Id = Guid.NewGuid(), Name = RoleTypeConstants.Teacher, NormalizedName = RoleTypeConstants.Teacher.ToUpper() },
        new() { Id = Guid.NewGuid(), Name = RoleTypeConstants.Officer, NormalizedName = RoleTypeConstants.Officer.ToUpper() }
    };
}
