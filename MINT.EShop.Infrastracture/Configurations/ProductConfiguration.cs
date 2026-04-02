using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Entities.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.ManagerId);

            builder.ToTable(t => t.HasCheckConstraint("CK_Product_Stock_Min", "\"Stock\" >= 0"));
        }
    }
}
