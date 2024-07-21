using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class SubLearningAreaConfiguration : IEntityTypeConfiguration<SubLearningArea>
{
    public void Configure(EntityTypeBuilder<SubLearningArea> builder)
    {
        builder.HasKey(sla => sla.Id);
        builder.Property(sla => sla.Id).IsRequired();
        builder.Property(sla => sla.Name).IsRequired();

        builder.HasMany(sla => sla.Benefits);

        builder.Property(sla => sla.CreatedDate).IsRequired();
        builder.Property(sla => sla.UpdatedDate);
        builder.Property(sla => sla.DeletedDate);

        builder.HasQueryFilter(sla => !sla.DeletedDate.HasValue);
    }
}


