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
        public Task<LoginResponse?> LoginAsync(LoginRequest request);
        public Task<LoginResponse> RefreshTokenAsync(RefreshRequest request);
    }
}
