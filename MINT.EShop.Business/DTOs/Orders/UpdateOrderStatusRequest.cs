using MINT.EShop.Core.Enums;

namespace MINT.EShop.Business.DTOs.Orders
{
    public record UpdateOrderStatusRequest
    {
        /// <summary>
        /// Новий стан замовлення.
        /// </summary>
        public required OrderStatus OrderStatus { get; init; }
    }
}
