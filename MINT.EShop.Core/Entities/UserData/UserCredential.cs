using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Entities.UserData
{
    public class UserCredential
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string PasswordHash { get; set; }
        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}