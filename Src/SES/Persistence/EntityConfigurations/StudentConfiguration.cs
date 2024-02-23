using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.SurName).IsRequired();
        builder.Property(e => e.ClassAge).IsRequired();
        builder.Property(e => e.ClassBranch).IsRequired();
        builder.Property(e => e.SchoolNumber).IsRequired();
        builder.Property(e => e.Description);

        builder.Property(e => e.SchoolId).IsRequired();
        builder.Property(e => e.TeacherId).IsRequired();
        builder.Property(e => e.ExamId).IsRequired();

        builder.HasOne(e => e.School);

        builder.HasMany(e => e.Exams);
        builder.HasMany(e => e.Teachers);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


