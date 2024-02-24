using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class AnalysisHeaderConfiguration : IEntityTypeConfiguration<AnalysisHeader>
{
    public void Configure(EntityTypeBuilder<AnalysisHeader> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.ClassAge).IsRequired();
        builder.Property(e => e.AltClass).IsRequired();
        builder.Property(e => e.ExamSemesterYear).IsRequired();
        builder.Property(e => e.LessonName).IsRequired();
        builder.Property(e => e.SemesterSession).IsRequired();
        builder.Property(e => e.ExamCode);
        builder.Property(e => e.FooterNote);


        builder.Property(e => e.BenefitId).IsRequired();
        builder.Property(e => e.QuestionId).IsRequired();
        builder.Property(e => e.TeacherId).IsRequired();
        builder.Property(e => e.PrincipalId).IsRequired();
        builder.Property(e => e.StudentQuizAnswerId).IsRequired();
        builder.Property(e => e.RefScorePerQuestions).IsRequired();

        builder.HasOne(e => e.Teacher);
        builder.HasOne(e => e.Principal);
        builder.HasOne(e => e.School);

        builder.HasMany(e => e.Questions);
        builder.HasMany(e => e.Benefits);
        builder.HasMany(e => e.StudentQuizAnswers);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


