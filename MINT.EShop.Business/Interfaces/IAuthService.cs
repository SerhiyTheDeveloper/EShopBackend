using MINT.EShop.Business.DTOs.Identity;

namespace MINT.EShop.Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<LoginResponse> RefreshTokenAsync(RefreshRequest request);
        Task LogoutAsync(string refreshToken);
    }
}
