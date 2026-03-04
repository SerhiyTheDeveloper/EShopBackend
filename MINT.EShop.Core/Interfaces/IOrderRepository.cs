using MINT.EShop.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        void Update(Order order);
        void Delete(Order order);
    }
}
