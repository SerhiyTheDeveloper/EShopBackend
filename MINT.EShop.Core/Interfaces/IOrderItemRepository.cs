using MINT.EShop.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetByIdAsync(Guid orderItemId);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task AddAsync(OrderItem orderItem);
        void Update(OrderItem orderItem);
        Task DeleteAsync(Guid orderItemId);
    }
}
