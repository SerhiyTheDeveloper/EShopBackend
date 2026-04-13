using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.HasKey(us => us.Id);

            builder.Property(us => us.RefreshToken)
                .IsRequired();

            builder.Property(us => us.ExpiresDate)
                .IsRequired();

            builder
                .HasOne(us => us.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(us => us.RefreshToken).IsUnique();
        }
    }
}
