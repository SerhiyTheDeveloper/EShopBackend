using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.Product;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
    {
        public async Task AddAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
        }

        public void Delete(Category category)
        {
            dbContext.Categories.Remove(category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public Task<Category?> GetByIdAsync(Guid categoryId)
        {
            return dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public void Update(Category category)
        {
            dbContext.Categories.Update(category);
        }
    }
}
