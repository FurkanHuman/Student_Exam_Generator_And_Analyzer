using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(p => p.PersonelId).IsRequired();
        builder.Ignore(p => p.Personel);
    }

    internal static Guid AdminGuid { get; set; } = Guid.NewGuid();

    private ApplicationUser getSeedUser()
    {
        ApplicationUser admin = new()
        {
            Id = AdminGuid,
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@root",
            NormalizedEmail = "ADMIN@ROOT",
            PhoneNumber = "1234567890",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            PersonelId = PersonelConfiguration.AdminPersonelId
        };

        return admin;
    }
}
