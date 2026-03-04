using MINT.EShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid productId);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> productIds);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
