using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MINT.EShop.Core.Entities.UserData;

namespace MINT.EShop.Core.Entities
{
    public class ClientAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? PhoneNumber { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Order.Order> Orders { get; set; } = [];
    }
}