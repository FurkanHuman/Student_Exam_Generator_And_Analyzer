using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Question).IsRequired();
        builder.Property(e => e.QuestionImage).IsRequired();

        builder.Property(e => e.BenefitId).IsRequired();
        builder.Property(e => e.ExamId).IsRequired();

        builder.HasMany(e => e.Exams);
        builder.HasMany(e => e.Benefits);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


