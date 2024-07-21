using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).IsRequired();
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.PrincipalId).IsRequired();

        builder.HasOne(s => s.Principal);

        builder.HasMany(s => s.Students);
        builder.HasMany(s => s.Teachers);
        builder.HasMany(s => s.Exams);
        builder.HasMany(s => s.Analysis);
        builder.HasMany(s => s.ReferenceBenefits);

        builder.Property(s => s.CreatedDate).IsRequired();
        builder.Property(s => s.UpdatedDate);
        builder.Property(s => s.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


