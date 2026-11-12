using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.Order;

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

            builder.Property(o => o.Region)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.DeliveryPoint)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.RecipientPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(o => o.ClientAccount)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ClientAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
