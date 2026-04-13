using MINT.EShop.Business.DTOs.Products;

namespace MINT.EShop.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync(GetProductsFilter filter);
        Task<ProductResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductResponse>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task<ProductResponse> CreateAsync(Guid managerId, CreateProductRequest request);
        Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
