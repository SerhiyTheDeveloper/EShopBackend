using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Interfaces;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class ProductRepository(AppDbContext dbContext) : IProductRepository
    {
        public async Task AddAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            dbContext.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(decimal? maxPrice = null, decimal? minPrice = null)
        {
            var query = dbContext.Products.AsQueryable();

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> productIds)
        {
            return await dbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        }

        public void Update(Product product)
        {
            dbContext.Products.Update(product);
        }
    }
}
