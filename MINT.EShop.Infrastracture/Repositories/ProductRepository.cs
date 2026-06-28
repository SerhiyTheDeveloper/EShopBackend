using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.Product;
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

        public async Task<IEnumerable<Product>> GetAllAsync(decimal? maxPrice = null, decimal? minPrice = null, string? category = null, string? producer = null)
        {
            var query = dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .AsQueryable();

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (category != null)
            {
                query = query.Where(p => p.Category.Slug == category);
            }

            if (producer != null)
            {
                query = query.Where(p => p.Producer.Slug == producer);
            }

            return await query.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> productIds)
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

        public void Update(Product product)
        {
            dbContext.Products.Update(product);
        }
    }
}
