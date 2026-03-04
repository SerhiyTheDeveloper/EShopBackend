using MINT.EShop.Business.DTOs.OrderItems;
using MINT.EShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.Orders
{
    public record OrderResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор (GUID).
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Ідентифікатор клієнта, який зробив замовлення (GUID).
        /// </summary>
        public required Guid ClientId { get; init; }
        /// <summary>
        /// Точна дата та час створення замовлення.
        /// </summary>
        /// <example>2024-06-01T14:30:00Z</example>
        public required DateTime OrderDate { get; init; }
        /// <summary>
        /// Остаточна вартість замовлення.
        /// </summary>
        /// <example>26999.99</example>
        public required decimal TotalAmount { get; init; }
        /// <summary>
        /// Стан замовлення.
        /// </summary>
        /// <example>Pending</example>
        public required OrderStatus Status { get; init; }
        /// <summary>
        /// Список товарів, включених до замовлення. (Тільки для читання)
        /// </summary>
        public required IReadOnlyList<OrderItemResponse> OrderItems { get; init; }
    }
}
