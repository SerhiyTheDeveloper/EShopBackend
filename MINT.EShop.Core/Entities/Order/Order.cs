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
        public required Guid ClientId { get; set; }
        public required DateTime OrderDate { get; set; }
        public required decimal TotalAmount { get; set; }
        public Client Client { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = [];
    }
}