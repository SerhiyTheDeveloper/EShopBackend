using MINT.EShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
