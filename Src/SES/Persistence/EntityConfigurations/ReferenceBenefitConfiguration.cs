using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class ReferenceBenefitConfiguration : IEntityTypeConfiguration<ReferenceBenefit>
{
    public void Configure(EntityTypeBuilder<ReferenceBenefit> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.ReferenceBenefitName).IsRequired();

        builder.Property(e => e.SchoolId).IsRequired();
        builder.Property(e => e.ExamId);
        builder.Property(e => e.LearningAreaId).IsRequired();
        builder.Property(e => e.SemesterId).IsRequired();

        builder.HasOne(e => e.School);
        builder.HasOne(e => e.Semester);

        builder.HasMany(e => e.QuizQuestions);
        builder.HasMany(e => e.LearningAreas);
        builder.HasMany(e => e.Exams);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


