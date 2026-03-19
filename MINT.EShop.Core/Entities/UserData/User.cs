using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MINT.EShop.Core.Entities.UserData
{
   public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public UserCredential Credential { get; set; } = null!;
        public List<UserSession> Sessions { get; set; } = [];
    }
}