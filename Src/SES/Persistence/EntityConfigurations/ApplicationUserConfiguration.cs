using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>, IUserConfiguration
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(p => p.PersonelId).IsRequired();
        builder.Ignore(p => p.Personel);
    }
}
