using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Business.DTOs.Orders;
using MINT.EShop.Core.Enums;

namespace MINT.EShop.Business.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync(Guid? clientAccountId, GetOrdersFilter filter);
        Task<OrderResponse?> GetByIdAsync(Guid clientAccountId, Guid orderId, Role userRole);
        Task<OrderResponse> CreateAsync(Guid userId, CreateOrderRequest request); 
        Task<bool> UpdateStatusAsync(Guid orderId, OrderStatus newStatus);
        Task<bool> CancelMyAsync(Guid clientId,  Guid orderId);
        Task<OrderResponse?> IncreaseOrderItemQuantityAsync(Guid orderId, Guid orderItemId);
        Task<OrderResponse?> DecreaseOrderItemQuantityAsync(Guid orderId, Guid orderItemId);
        Task<OrderResponse?> AddOrderItemAsync(Guid orderId, CreateOrderItemRequest orderItemRequest);
        Task<bool> DeleteOrderItemAsync(Guid orderId, Guid orderItemId);
    }
}
