using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class SubLearningAreaConfiguration : IEntityTypeConfiguration<SubLearningArea>
{
    public void Configure(EntityTypeBuilder<SubLearningArea> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.BenefitId).IsRequired();

        builder.HasMany(e => e.Benefits);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


