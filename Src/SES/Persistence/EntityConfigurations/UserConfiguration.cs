using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NArchitecture.Core.Security.Hashing;

namespace Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).IsRequired();        
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.PasswordSalt).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.AuthenticatorType).IsRequired();

        builder.Property(u => u.CreatedDate).IsRequired();
        builder.Property(u => u.UpdatedDate);
        builder.Property(u => u.DeletedDate);

        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);


        builder.HasOne(u => u.Personel).WithOne(p => p.User).HasForeignKey<User>(u => u.PersonelId);
            ;

        builder.HasMany(u => u.UserOperationClaims);
        builder.HasMany(u => u.RefreshTokens);
        builder.HasMany(u => u.EmailAuthenticators);
        builder.HasMany(u => u.OtpAuthenticators);

        builder.HasData(_seeds);

        builder.HasBaseType((string)null!);
    }

    public static Guid AdminId { get; } = Guid.NewGuid();
    private IEnumerable<User> _seeds
    {
        get
        {
            HashingHelper.CreatePasswordHash(
                password: "Passw0rd!",
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );

            yield return new()
            {
                Id = AdminId,
                PersonelId=PersonelConfiguration.AdminPersonelId,
                Email = "furkan@human.app",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
        }
    }
}
