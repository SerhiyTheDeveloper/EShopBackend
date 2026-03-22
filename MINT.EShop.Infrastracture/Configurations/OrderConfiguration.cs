using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.OrderDate)
                .IsRequired();

            // Зв'язкок до акаунта клієнта
        }
    }
}
