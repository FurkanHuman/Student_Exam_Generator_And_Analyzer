using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).IsRequired();
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.BeginSemesterDate).IsRequired();
        builder.Property(s => s.EndSemesterDate).IsRequired();

        builder.HasMany(s => s.Analyses);
        builder.HasMany(s => s.Exams);
        builder.HasMany(s => s.Principals);
        builder.HasMany(s => s.ReferenceBenefits);
        builder.HasMany(s => s.Teachers);
        builder.HasMany(s => s.Students);


        builder.Property(s => s.CreatedDate).IsRequired();
        builder.Property(s => s.UpdatedDate);
        builder.Property(s => s.DeletedDate);

        builder.HasQueryFilter(s => !s.DeletedDate.HasValue);
    }
}
