using Microsoft.AspNetCore.Identity;
using MINT.EShop.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            return passwordHash;
        }

        public bool Verify(string password, string passwordHash)
        {
            bool isMatched = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
            return isMatched;
        }
    }
}
