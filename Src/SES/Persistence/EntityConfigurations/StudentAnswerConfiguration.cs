using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    internal class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.HasKey(sa => sa.Id);
            builder.Property(sa => sa.AnswerText);
            builder.Property(sa => sa.Score).IsRequired();
            builder.Property(sa => sa.IsCorrect).IsRequired();

            builder.Property(sa => sa.StudentId).IsRequired();
            builder.Property(sa => sa.QuizQuestionId).IsRequired();
            builder.Property(sa => sa.QuestionOptionId);

            builder.HasOne(sa => sa.Student);
            builder.HasOne(sa => sa.QuizQuestion);
            builder.HasOne(sa => sa.QuestionOption);

            builder.Property(sa => sa.CreatedDate).IsRequired();
            builder.Property(sa => sa.UpdatedDate);
            builder.Property(sa => sa.DeletedDate);

            builder.HasQueryFilter(sa => !sa.DeletedDate.HasValue);
        }
    }
}
