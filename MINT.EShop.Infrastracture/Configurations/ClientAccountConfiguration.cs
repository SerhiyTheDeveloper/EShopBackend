using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class ClientAccountConfiguration : IEntityTypeConfiguration<ClientAccount>
    {
        public void Configure(EntityTypeBuilder<ClientAccount> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithOne(u => u.ClientAccount)
                .HasForeignKey<ClientAccount>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
