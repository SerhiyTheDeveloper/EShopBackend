using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Core.Interfaces
{
    public interface IWishlistItemRepository
    {
        Task<WishlistItem?> GetByIdAsync(Guid clientId, Guid productId);
        Task<List<WishlistItem>> GetAllAsync(Guid clientId);
        Task AddAsync(WishlistItem wishlistItem);
        void Delete(WishlistItem wishlistItem);
    }
}
