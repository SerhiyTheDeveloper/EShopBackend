using MINT.EShop.Business.DTOs.Identity;

namespace MINT.EShop.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<LoginResponse?> LoginAsync(LoginRequest request);
        public Task<LoginResponse> RefreshTokenAsync(RefreshRequest request);
    }
}
