using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class AnalysisConfiguration : IEntityTypeConfiguration<Analysis>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<Analysis> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).IsRequired();
        builder.Property(a => a.AIResponse);
        builder.Property(a => a.SemesterId).IsRequired();
        builder.Property(a => a.PrincipalId).IsRequired();
        builder.Property(a => a.SchoolId).IsRequired();
        builder.Property(a => a.LessonId).IsRequired();
        builder.Property(a => a.ReferenceBenefitId).IsRequired();

        builder.HasOne(a => a.Semester);
        builder.HasOne(a => a.Principal);
        builder.HasOne(a => a.School);
        builder.HasOne(a => a.Lesson);
        builder.HasOne(a => a.ReferenceBenefit);

        builder.HasMany(a => a.Exams);
        builder.HasMany(a => a.StudentExamAnswers);
        builder.HasMany(a => a.Teachers);

        builder.Property(a => a.CreatedDate).IsRequired();
        builder.Property(a => a.UpdatedDate);
        builder.Property(a => a.DeletedDate);

        builder.HasQueryFilter(a => !a.DeletedDate.HasValue);
    }
}
