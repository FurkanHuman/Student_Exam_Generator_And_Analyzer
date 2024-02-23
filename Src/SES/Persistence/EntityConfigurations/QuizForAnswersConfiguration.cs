using Entity.Entities.Mains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class QuizForAnswersConfiguration : IEntityTypeConfiguration<QuizForAnswers>
{
    public void Configure(EntityTypeBuilder<QuizForAnswers> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();

        builder.HasMany(e => e.QuizAnswers);

        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.UpdatedDate);
        builder.Property(e => e.DeletedDate);

        builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
    }
}


