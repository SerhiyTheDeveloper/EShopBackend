using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.OrderItems
{
    public record OrderItemResponse
    {
        /// <summary>
        /// Унікальний ідентифікатор (GUID).
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Ідентифікатор товару, який замовляється (GUID).
        /// </summary>
        public required Guid ProductId { get; init; }
        /// <summary>
        /// Ідентифікатор замовлення, до якого належить цей товар (GUID).
        /// </summary>
        public required Guid OrderId { get; init; }
        /// <summary>
        /// Кількість одиниць товару.
        /// </summary>
        /// example>2</example>
        public required int Quantity { get; init; }
        /// <summary>
        /// Ціна за одиницю товару на момент замовлення.
        /// </summary>
        /// <example>26999.99</example>
        public required decimal UnitPrice { get; init; }
        /// <summary>
        /// Знижка на товар у відсотках.
        /// </summary>
        /// <example>10</example>
        public required decimal Discount { get; init; }
    }
}
