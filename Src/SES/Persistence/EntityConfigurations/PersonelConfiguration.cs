using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class PersonelConfiguration : IEntityTypeConfiguration<Personel>
{
    public void Configure(EntityTypeBuilder<Personel> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.SurName).IsRequired();
        builder.Property(p => p.BirthDate).IsRequired();

        builder.HasOne(p => p.User);

        builder.Property(p => p.CreatedDate).IsRequired();
        builder.Property(p => p.UpdatedDate);
        builder.Property(p => p.DeletedDate);

        builder.HasQueryFilter(p => !p.DeletedDate.HasValue);
        builder.HasData(_seeds);
    }

    public static Guid AdminPersonelId { get; set; } = Guid.NewGuid();

    private IEnumerable<Personel> _seeds
    {
        get
        {
            yield return new()
            {
                Id = AdminPersonelId,
                UserId = UserConfiguration.AdminId,
                Name = "Admin",
                SurName = "Administrator",
                BirthDate = DateOnly.FromDateTime(DateTime.Now),
            };
        }
    }
}