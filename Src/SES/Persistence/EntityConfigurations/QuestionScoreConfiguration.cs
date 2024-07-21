using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class QuestionScoreConfiguration : IEntityTypeConfiguration<QuestionScore>
{
    public void Configure(EntityTypeBuilder<QuestionScore> builder)
    {
        builder.HasKey(qs => qs.Id);
        builder.Property(qs => qs.Id).IsRequired();
        builder.Property(qs => qs.Score).IsRequired();
        builder.Property(qs => qs.MaxScore).IsRequired();

        builder.HasMany(qs => qs.QuizQuestions);
        builder.HasMany(qs => qs.StudentAnswers);

        builder.Property(qs => qs.CreatedDate).IsRequired();
        builder.Property(qs => qs.UpdatedDate);
        builder.Property(qs => qs.DeletedDate);

        builder.HasQueryFilter(qs => !qs.DeletedDate.HasValue);
    }
}


