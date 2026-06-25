using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.HasKey(w => new { w.ClientId, w.ProductId });

            builder.HasOne(w => w.ClientAccount)
                .WithMany(cl => cl.Wishlist)
                .HasForeignKey(w => w.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
