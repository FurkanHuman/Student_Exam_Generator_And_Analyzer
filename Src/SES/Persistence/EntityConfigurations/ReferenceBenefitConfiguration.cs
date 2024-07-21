using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class ReferenceBenefitConfiguration : IEntityTypeConfiguration<ReferenceBenefit>
{
    public void Configure(EntityTypeBuilder<ReferenceBenefit> builder)
    {
        builder.HasKey(rb => rb.Id);
        builder.Property(rb => rb.Id).IsRequired();
        builder.Property(rb => rb.ReferenceBenefitName).IsRequired();
        builder.Property(rb => rb.LessonId).IsRequired();
        builder.Property(rb => rb.SchoolId).IsRequired();
        builder.Property(rb => rb.SemesterId).IsRequired();

        builder.HasOne(rb => rb.Lesson);
        builder.HasOne(rb => rb.School);
        builder.HasOne(rb => rb.Semester);

        builder.HasMany(rb => rb.Exams);
        builder.HasMany(rb => rb.QuizQuestions);
        builder.HasMany(rb => rb.LearningAreas);

        builder.Property(rb => rb.CreatedDate).IsRequired();
        builder.Property(rb => rb.UpdatedDate);
        builder.Property(rb => rb.DeletedDate);

        builder.HasQueryFilter(rb => !rb.DeletedDate.HasValue);
    }
}


