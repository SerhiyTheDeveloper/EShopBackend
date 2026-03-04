using MINT.EShop.Business.DTOs.Orders;
using MINT.EShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByIdAsync(Guid id);
        Task<OrderResponse> CreateAsync(CreateOrderRequest request); 
        Task<bool> UpdateStatusAsync(Guid id, OrderStatus newStatus);
    }
}
