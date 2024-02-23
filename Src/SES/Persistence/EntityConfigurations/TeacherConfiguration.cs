using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.SurName).IsRequired();

        builder.Property(e => e.ExamId).IsRequired(); 
        builder.Property(e => e.StudentId).IsRequired();
        builder.Property(e => e.SchoolId).IsRequired();

        builder.HasMany(e => e.ReferenceBenefits);
        builder.HasMany(e => e.Exams);
        builder.HasMany(e => e.Students);
        builder.HasMany(e => e.Schools);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


