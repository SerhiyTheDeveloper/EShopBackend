using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.OrderItems
{
    public record CreateOrderItemRequest
    {
        /// <summary>
        /// Унікальний ідентифікатор (GUID).
        /// </summary>
        public required Guid ProductId { get; init; }
        /// <summary>
        /// Кількість одиниць товару.
        /// </summary>
        /// <example>2</example>
        public required int Quantity { get; init; }
    }
}
