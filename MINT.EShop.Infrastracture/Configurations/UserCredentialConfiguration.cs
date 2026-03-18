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
