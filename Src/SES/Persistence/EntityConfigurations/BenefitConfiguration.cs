using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class BenefitConfiguration : IEntityTypeConfiguration<Benefit>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Benefit> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.BenefitCode).IsRequired();
        builder.Property(e => e.Description).IsRequired();

        builder.HasOne(e => e.SubLearningArea);

        builder.HasMany(e => e.QuizQuestions);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


