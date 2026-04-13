using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
    {
        public void Configure(EntityTypeBuilder<UserCredential> builder)
        {
            builder.HasKey(uc => uc.Id);

            builder.Property(uc => uc.PasswordHash)
                .IsRequired();

            builder
                .HasOne(uc => uc.User)
                .WithOne(u => u.Credential)
                .HasForeignKey<UserCredential>(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
