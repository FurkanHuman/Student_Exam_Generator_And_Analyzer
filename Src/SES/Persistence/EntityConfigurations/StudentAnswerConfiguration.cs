using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<StudentAnswer> builder)
    {
        builder.HasKey(sa => sa.Id);

        builder.Property(sa => sa.StudentExamAnswerId).IsRequired();
        builder.Property(sa => sa.QuizQuestionId).IsRequired();
        builder.Property(sa => sa.EvaluationStatus).IsRequired();
        builder.Property(sa => sa.QuestionOptionId);
        
        builder.Property(sa => sa.AnswerText);
        builder.Property(sa => sa.GivenScore);

        builder.HasOne(sa => sa.QuizQuestion);
        builder.HasOne(sa => sa.QuestionOption);
        builder.HasOne(sa => sa.StudentExamAnswer);

        builder.Property(sa => sa.CreatedDate).IsRequired();
        builder.Property(sa => sa.UpdatedDate);
        builder.Property(sa => sa.DeletedDate);

        builder.HasQueryFilter(sa => !sa.DeletedDate.HasValue);
    }
}
