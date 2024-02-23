using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired();


        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


