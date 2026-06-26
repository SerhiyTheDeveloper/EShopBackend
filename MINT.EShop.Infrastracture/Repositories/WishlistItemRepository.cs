using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class WishListItemRepository(AppDbContext dbContext) : IWishListItemRepository
    {
        public async Task AddAsync(WishListItem wishListItem)
        {
            await dbContext.WishListItems.AddAsync(wishListItem);
        }

        public async Task<WishListItem?> GetByIdAsync(Guid clientId, Guid productId)
        {
            return await dbContext.WishListItems
                .FirstOrDefaultAsync(wli => wli.ClientId == clientId && wli.ProductId == productId);
        }

        public async Task<List<WishListItem>> GetAllAsync(Guid clientId)
        {
            return await dbContext.WishListItems
                .Where(wli => wli.ClientId == clientId)
                .ToListAsync();
        }

        public void Delete(WishListItem wishListItem)
        {
            dbContext.WishListItems.Remove(wishListItem);
        }
    }
}
