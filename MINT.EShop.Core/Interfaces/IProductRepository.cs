using MINT.EShop.Core.Entities.Product;

namespace MINT.EShop.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid productId);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> productIds);
        Task<IEnumerable<Product>> GetAllAsync(decimal? maxPrice = null, decimal? minPrice = null, Guid? category = null, Guid? producer = null);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
