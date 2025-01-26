using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class SubLearningAreaConfiguration : IEntityTypeConfiguration<SubLearningArea>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<SubLearningArea> builder)
    {
        builder.HasKey(sla => sla.Id);
        builder.Property(sla => sla.Id).IsRequired();
        builder.Property(sla => sla.SLACode).IsRequired();
        builder.Property(sla => sla.Description).IsRequired();

        builder.HasMany(sla => sla.Benefits);

        builder.Property(sla => sla.CreatedDate).IsRequired();
        builder.Property(sla => sla.UpdatedDate);
        builder.Property(sla => sla.DeletedDate);

        builder.HasQueryFilter(sla => !sla.DeletedDate.HasValue);
    }
}


