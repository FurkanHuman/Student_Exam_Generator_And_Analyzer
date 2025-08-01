using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class TeacherConfiguration : IEntityTypeConfiguration<Teacher>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).IsRequired();

        builder.Property(t => t.PersonelId).IsRequired();
        builder.Property(t => t.SchoolId).IsRequired();
        builder.Property(t => t.SemesterId).IsRequired();

        builder.HasOne(t => t.Personel);
        builder.HasOne(t => t.School);
        builder.HasOne(t => t.Semester);

        builder.HasMany(t => t.ReferenceBenefits);
        builder.HasMany(t => t.Exams);
        builder.HasMany(t => t.ExamAuthors).WithOne(e => e.ExamAuthor);
        builder.HasMany(t => t.StudentAnswers);
        builder.HasMany(t => t.Students);
        builder.HasMany(t => t.Lessons);

        builder.Property(t => t.CreatedDate).IsRequired();
        builder.Property(t => t.UpdatedDate);
        builder.Property(t => t.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


