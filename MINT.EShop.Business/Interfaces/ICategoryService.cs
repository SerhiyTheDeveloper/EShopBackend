using MINT.EShop.Business.DTOs.Category;

namespace MINT.EShop.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse?> GetByIdAsync(Guid id);
        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
        Task<CategoryResponse?> UpdateAsync(Guid id, UpdateCategoryRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
