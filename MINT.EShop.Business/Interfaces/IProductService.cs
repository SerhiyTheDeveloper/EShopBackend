using MINT.EShop.Business.DTOs.Products;
using MINT.EShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync();
        Task<ProductResponse?> GetByIdAsync(Guid id);
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
