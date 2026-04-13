using MINT.EShop.Core.Enums;

namespace MINT.EShop.Business.DTOs.Orders
{
    public record GetOrdersFilter
    {
        /// <summary>
        /// Статус замовлення (Cancelled, Pending, Paid, Shipped, Delivered).
        /// </summary>
        public OrderStatus? Status { get; init; }
    }
}
