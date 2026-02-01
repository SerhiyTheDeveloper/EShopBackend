using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MINT.EShop.Core.Entities
{
   public class User : IdentityUser<Guid>
    {
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public Client? Client { get; set; }
    }
}
