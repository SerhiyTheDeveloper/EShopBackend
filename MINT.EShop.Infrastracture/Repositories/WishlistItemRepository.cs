using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class WishlistItemRepository(AppDbContext dbContext) : IWishlistItemRepository
    {
        public async Task AddAsync(WishlistItem wishlistItem)
        {
            await dbContext.WishlistItems.AddAsync(wishlistItem);
        }

        public async Task<WishlistItem?> GetByIdAsync(Guid clientId, Guid productId)
        {
            return await dbContext.WishlistItems
                .FirstOrDefaultAsync(wli => wli.ClientId == clientId && wli.ProductId == productId);
        }

        public async Task<List<WishlistItem>> GetAllAsync(Guid clientId)
        {
            return await dbContext.WishlistItems
                .Where(wli => wli.ClientId == clientId)
                .ToListAsync();
        }

        public void Delete(WishlistItem wishlistItem)
        {
            dbContext.WishlistItems.Remove(wishlistItem);
        }
    }
}
