using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.HasKey(qq => qq.Id);
        builder.Property(qq => qq.Id).IsRequired();
        builder.Property(qq => qq.Score).IsRequired();
        builder.Property(qq => qq.Question).IsRequired();
        builder.Property(qq => qq.QuestionBody).IsRequired();
        builder.Property(qq => qq.QuestionImage).IsRequired();
        builder.Property(qq => qq.QuestionType).IsRequired();

        builder.Property(qq => qq.BenefitId).IsRequired();
        builder.Property(qq => qq.ExamId).IsRequired();

        builder.HasMany(qq => qq.Exams);
        builder.HasMany(qq => qq.Benefits);
        builder.HasMany(qq => qq.Options);
        builder.HasMany(qq => qq.StudentAnswers);

        builder.Property(qq => qq.CreatedDate).IsRequired();
        builder.Property(qq => qq.UpdatedDate);
        builder.Property(qq => qq.DeletedDate);

        builder.HasQueryFilter(qq => !qq.DeletedDate.HasValue);
    }
}


