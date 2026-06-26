using MINT.EShop.Business.DTOs.Products;

namespace MINT.EShop.Business.Interfaces
{
    public interface IWishListService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync(Guid clientId);
        Task<ProductResponse?> GetByIdAsync(Guid clientId, Guid productId);
        Task AddAsync(Guid clientId, Guid productId);
        Task DeleteAsync(Guid clientId, Guid productId);
    }
}
