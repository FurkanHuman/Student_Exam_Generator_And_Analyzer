using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

public class PersonelConfiguration : IEntityTypeConfiguration<Personel>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Personel> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.SurName).IsRequired();
        builder.Property(p => p.BirthDate).IsRequired();
        builder.Ignore(p => p.User);

        builder.Property(p => p.CreatedDate).IsRequired();
        builder.Property(p => p.UpdatedDate);
        builder.Property(p => p.DeletedDate);

        builder.HasQueryFilter(p => !p.DeletedDate.HasValue);
    }

    internal static Guid AdminPersonelId { get; set; } = Guid.NewGuid();

    private Personel getSeedPersonel()
    {
        Personel personel = new()
        {
            Id = AdminPersonelId,
            UserId = ApplicationUserConfiguration.AdminGuid,
            Name = "Admin",
            SurName = "Administrator",
            BirthDate = DateOnly.FromDateTime(DateTime.Now),
        };
        return personel;

    }
}
