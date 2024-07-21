using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class PrincipalConfiguration : IEntityTypeConfiguration<Principal>
{
    public void Configure(EntityTypeBuilder<Principal> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p=>p.Id).IsRequired();
        builder.Property(p => p.SemesterId).IsRequired();

        builder.HasOne(p => p.Personel);
        builder.HasOne(p => p.School);
        builder.HasOne(p => p.Semester);

        builder.Property(p=>p.CreatedDate).IsRequired();
        builder.Property(p=>p.UpdatedDate);
        builder.Property(p=>p.DeletedDate);

        builder.HasQueryFilter(p => !p.DeletedDate.HasValue);
    }

}


