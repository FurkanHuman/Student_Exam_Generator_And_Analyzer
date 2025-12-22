using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class StudentExamAnswerConfiguration : IEntityTypeConfiguration<StudentExamAnswer>
{
    public void Configure(EntityTypeBuilder<StudentExamAnswer> builder)
    {
        builder.HasKey(sea => sea.Id);

        builder.Property(sea => sea.Id).IsRequired();
        builder.Property(sea => sea.ReviewerTeacherId).IsRequired();
        builder.Property(sea => sea.StudentId).IsRequired();
        builder.Property(sea => sea.ExamId).IsRequired();
        builder.Property(sea => sea.EvaluationOrigin).IsRequired();
        builder.Property(sea => sea.ExamEvaluationStatus).IsRequired();

        builder.HasOne(sea => sea.ReviewerTeacher);
        builder.HasOne(sea => sea.Student);
        builder.HasOne(sea => sea.Exam);

        builder.HasMany(sea => sea.StudentAnswers);

        builder.Property(sea => sea.CreatedDate).IsRequired();
        builder.Property(sea => sea.UpdatedDate);
        builder.Property(sea => sea.DeletedDate);

        builder.HasQueryFilter(sea => !sea.DeletedDate.HasValue && sea.DeletedDate.HasValue);
    }
}