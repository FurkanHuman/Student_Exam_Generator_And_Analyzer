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
        builder.Property(e => e.LessonName).IsRequired();
        builder.Property(e => e.ExamCode).IsRequired();
        builder.Property(e => e.FooterNote);

        builder.Property(e => e.AnalysisId).IsRequired();
        builder.Property(e => e.TeacherId).IsRequired();
        builder.Property(e => e.StudentId).IsRequired();
        builder.Property(e => e.SchoolId);
        builder.Property(e => e.ReferenceBenefitId).IsRequired();
        builder.Property(e => e.SemesterId).IsRequired();

        builder.HasOne(e => e.Teacher);
        builder.HasOne(e => e.Student);
        builder.HasOne(e => e.School);
        builder.HasOne(e => e.ReferenceBenefit);
        builder.HasOne(e => e.Analysis);
        builder.HasOne(e => e.Semester);


        builder.HasMany(e => e.QuizQuestions);


        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


