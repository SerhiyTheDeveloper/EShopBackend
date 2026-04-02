using MINT.EShop.Business.DTOs.OrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Orders
{
    public record CreateOrderRequest
    {
        /// <summary>
        /// Список товарів у замовленні.
        /// </summary>
        public required IReadOnlyList<CreateOrderItemRequest> OrderItems { get; init; }
    }
}
