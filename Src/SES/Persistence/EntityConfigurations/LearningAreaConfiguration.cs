using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class LearningAreaConfiguration : IEntityTypeConfiguration<LearningArea>
{
    public void Configure(EntityTypeBuilder<LearningArea> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.SubLearningAreaId).IsRequired();

        builder.HasMany(e => e.SubLearningAreas);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


