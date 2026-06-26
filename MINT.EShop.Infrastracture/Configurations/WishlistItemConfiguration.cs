using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Infrastracture.Configurations
{
    public class WishListItemConfiguration : IEntityTypeConfiguration<WishListItem>
    {
        public void Configure(EntityTypeBuilder<WishListItem> builder)
        {
            builder.HasKey(w => new { w.ClientId, w.ProductId });

            builder.HasOne(w => w.ClientAccount)
                .WithMany(cl => cl.Wishlist)
                .HasForeignKey(w => w.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
