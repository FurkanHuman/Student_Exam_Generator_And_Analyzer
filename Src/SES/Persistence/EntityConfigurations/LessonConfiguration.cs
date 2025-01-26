using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).IsRequired();
        builder.Property(l => l.LessonName).IsRequired();
        builder.Property(l => l.Description).IsRequired();
        builder.Property(l => l.SemesterId).IsRequired();

        builder.HasOne(l => l.Semester);

        builder.HasMany(l => l.StudentClasses);
        builder.HasMany(l => l.ReferenceBenefits);
        builder.HasMany(l => l.Teachers);

        builder.Property(l => l.CreatedDate).IsRequired();
        builder.Property(l => l.UpdatedDate);
        builder.Property(l => l.DeletedDate);

        builder.HasQueryFilter(l => !l.DeletedDate.HasValue);
    }
}