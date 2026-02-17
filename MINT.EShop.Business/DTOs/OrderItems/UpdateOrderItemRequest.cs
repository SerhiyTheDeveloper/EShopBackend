using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.DTOs.OrderItems
{
    public record UpdateOrderItemRequest
    {
        /// <summary>
        /// Кількість одиниць товару.
        /// </summary>
        /// <example>2</example>
        public required int Quantity { get; init; }
    }
}
