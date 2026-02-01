using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = [];

        public Task AddAsync(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid productId)
        {
            _products.RemoveAll(p => p.Id == productId);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Product>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Product>>(_products);

        public Task<Product?> GetByIdAsync(Guid productId) =>
            Task.FromResult(_products.FirstOrDefault(p => p.Id == productId));

        public Task UpdateAsync(Product product) =>
            Task.CompletedTask;
    }
}
