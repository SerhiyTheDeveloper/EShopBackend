using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Core.Interfaces
{
    public interface IWishListItemRepository
    {
        Task<WishListItem?> GetByIdAsync(Guid clientId, Guid productId);
        Task<List<WishListItem>> GetAllAsync(Guid clientId);
        Task AddAsync(WishListItem wishListItem);
        void Delete(WishListItem wishListItem);
    }
}
