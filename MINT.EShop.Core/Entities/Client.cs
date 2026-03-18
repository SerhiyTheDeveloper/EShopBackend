using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Core.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Order.Order> Orders { get; set; } = [];
    }
}
