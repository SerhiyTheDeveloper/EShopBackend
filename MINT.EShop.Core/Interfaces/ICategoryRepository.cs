using MINT.EShop.Core.Entities.Product;

namespace MINT.EShop.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid categoryId);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
