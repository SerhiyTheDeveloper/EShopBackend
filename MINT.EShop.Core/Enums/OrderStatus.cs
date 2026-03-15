using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Paid = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 0
    }
}