using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;


namespace Persistence.EntityConfigurations;

internal class ExamConfigurationConfiguration : IEntityTypeConfiguration<Domain.Entities.ExamConfiguration>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ExamConfiguration> builder)
    {
        builder.HasKey(ec => ec.Id);
        builder.Property(ec => ec.Id).IsRequired();
        builder.Property(ec => ec.ConfigurationJsonStr).IsRequired();
        builder.Property(ec => ec.ConfigurationHash).IsRequired().HasMaxLength(64);// burası üstekinin hash in alacak

        builder.HasMany(ec => ec.Exams);

        builder.Property(ec => ec.CreatedDate).IsRequired();
        builder.Property(ec => ec.UpdatedDate);
        builder.Property(ec => ec.DeletedDate);

        builder.HasQueryFilter(ec => !ec.DeletedDate.HasValue);
        builder.HasIndex(ec => ec.ConfigurationHash).IsUnique();
    }
}