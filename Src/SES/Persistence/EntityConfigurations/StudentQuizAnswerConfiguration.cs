using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class StudentQuizAnswerConfiguration : IEntityTypeConfiguration<StudentQuizAnswer>
{
    public void Configure(EntityTypeBuilder<StudentQuizAnswer> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.StudentNumber).IsRequired();
        builder.Property(e => e.TotalScore).IsRequired();
        builder.Property(e => e.SpecialStatuses);

        builder.Property(e => e.ExamId).IsRequired();

        builder.HasOne(e => e.Student);
        builder.HasOne(e => e.Exam);

        builder.HasMany(e => e.ScorePerQuestions);


        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


