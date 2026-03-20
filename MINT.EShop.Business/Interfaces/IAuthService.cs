using MINT.EShop.Business.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<LoginResponse?> Login(LoginRequest request);
    }
}
