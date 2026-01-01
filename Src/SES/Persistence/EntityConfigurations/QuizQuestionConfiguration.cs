using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.EntityConfigurations.Interfaces;

namespace Persistence.EntityConfigurations;

internal class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>, IMainConfiguration
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.HasKey(qq => qq.Id);
        builder.Property(qq => qq.Id).IsRequired();
        builder.Property(qq => qq.Question).IsRequired();
        builder.Property(qq => qq.QuestionBody).IsRequired(false);
        builder.Property(qq => qq.QuestionImageURL).IsRequired(false);
        builder.Property(qq => qq.QuestionType).IsRequired();
        builder.Property(qq => qq.IsAIGenerated).HasDefaultValue(false);
        builder.Property(qq=>   qq.PreviousQuestionId).IsRequired(false);

        builder.HasOne(qq => qq.QuestionScore);
        builder.HasOne(qq => qq.PreviousQuestion);

        builder.HasMany(qq => qq.Exams);
        builder.HasMany(qq => qq.Benefits);
        builder.HasMany(qq => qq.Options);
        builder.HasMany(qq => qq.StudentAnswers);
        builder.HasMany(qq => qq.Lessons);

        builder.Property(qq => qq.CreatedDate).IsRequired();
        builder.Property(qq => qq.UpdatedDate);
        builder.Property(qq => qq.DeletedDate);

        builder.HasQueryFilter(qq => !qq.DeletedDate.HasValue);
    }
}


