using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

internal class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
{
    public void Configure(EntityTypeBuilder<QuestionOption> builder)
    {
        builder.HasKey(qo => qo.Id);
        builder.Property(qo => qo.Id).IsRequired();
        builder.Property(qo => qo.QuizQuestionId).IsRequired();
        builder.Property(qo => qo.OptionText);
        builder.Property(qo => qo.IsCorrect).IsRequired();

        builder.HasOne(qo => qo.QuizQuestion);

        builder.Property(qo => qo.CreatedDate).IsRequired();
        builder.Property(qo => qo.UpdatedDate);
        builder.Property(qo => qo.DeletedDate);

        builder.HasQueryFilter(qo => !qo.DeletedDate.HasValue);
    }
}
