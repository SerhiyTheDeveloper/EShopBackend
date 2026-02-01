using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Entities.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required DateTime OrderDate { get; set; }
        public required decimal TotalAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; } = [];
    }
}
