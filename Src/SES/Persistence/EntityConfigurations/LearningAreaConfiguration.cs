using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class LearningAreaConfiguration : IEntityTypeConfiguration<LearningArea>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<LearningArea> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.LACode).IsRequired();
        builder.Property(e => e.Description).IsRequired();

        builder.HasMany(e => e.SubLearningAreas);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


