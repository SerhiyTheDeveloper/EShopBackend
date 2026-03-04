using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class ProductRepository(AppDbContext dbContext) : IProductRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            _dbContext.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> productIds)
        {
            return await _dbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        }

        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
        }
    }
}
