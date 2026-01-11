using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
{
    public void Configure(EntityTypeBuilder<FeedBack> builder)
    {
        builder.HasKey(fb => fb.Id);

        builder.Property(fb => fb.Id).IsRequired();
        builder.Property(fb => fb.UserName);
        builder.Property(fb => fb.Email).IsRequired();
        builder.Property(fb => fb.PageUrl).IsRequired();
        builder.Property(fb => fb.Message).IsRequired();
        builder.Property(fb => fb.SubmittedAt).IsRequired();
        builder.Property(fb => fb.CreatedDate).IsRequired();
        builder.Property(fb => fb.UpdatedDate);
        builder.Property(fb => fb.DeletedDate);

        builder.HasQueryFilter(fb => !fb.DeletedDate.HasValue);
    }
}