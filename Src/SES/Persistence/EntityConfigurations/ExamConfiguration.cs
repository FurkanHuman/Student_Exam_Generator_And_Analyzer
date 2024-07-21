using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.ExamLessonName).IsRequired();
        builder.Property(e => e.ExamCode).IsRequired();
        builder.Property(e => e.TotalScoreForString);
        builder.Property(e => e.TotalScore);


        builder.Property(e => e.LessonId).IsRequired();
        builder.Property(e => e.SemesterId).IsRequired();
        builder.Property(e => e.StudentId).IsRequired();
        builder.Property(e => e.SchoolId).IsRequired();
        builder.Property(e => e.ReferenceBenefitId).IsRequired();

        builder.HasOne(e => e.Lesson);
        builder.HasOne(e => e.Semester);
        builder.HasOne(e => e.Student);
        builder.HasOne(e => e.School);
        builder.HasOne(e => e.ReferenceBenefit);

        builder.HasMany(e => e.Analyses);
        builder.HasMany(e => e.Teachers);
        builder.HasMany(e => e.StudentClasses);
        builder.HasMany(e => e.QuizQuestions);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}