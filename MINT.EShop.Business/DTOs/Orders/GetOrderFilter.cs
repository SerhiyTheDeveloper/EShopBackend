using MINT.EShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
