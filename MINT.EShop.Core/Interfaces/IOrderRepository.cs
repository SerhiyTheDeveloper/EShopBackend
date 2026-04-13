using MINT.EShop.Core.Entities.Order;
using MINT.EShop.Core.Enums;

namespace MINT.EShop.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllAsync(Guid? clientAccountId = null, OrderStatus? status = null);
        Task AddAsync(Order order);
        void Update(Order order);
        void Delete(Order order);
        void AddOrderItem(OrderItem item);
    }
}
