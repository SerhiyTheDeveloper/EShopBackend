using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class ClientAccountConfiguration : IEntityTypeConfiguration<ClientAccount>
    {
        public void Configure(EntityTypeBuilder<ClientAccount> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            builder.HasOne(c => c.User)
                .WithOne(u => u.ClientAccount)
                .HasForeignKey<ClientAccount>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
