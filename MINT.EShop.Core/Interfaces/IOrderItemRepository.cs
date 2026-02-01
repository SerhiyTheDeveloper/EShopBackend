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
        Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task AddOrderItemAsync(OrderItem orderItem);
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(Guid orderItemId);
    }
}
