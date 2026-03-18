using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .WithMany(u => u.UserSessions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(us => us.RefreshToken).IsUnique();
        }
    }
}
