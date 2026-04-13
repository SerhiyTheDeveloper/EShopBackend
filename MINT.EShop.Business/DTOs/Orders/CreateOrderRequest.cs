using MINT.EShop.Business.DTOs.OrderItems;

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
