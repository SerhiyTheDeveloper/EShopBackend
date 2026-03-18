using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Core.Entities.UserData
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime ExpiresDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public bool IsActive => DateTime.UtcNow < ExpiresDate;
    }
}
