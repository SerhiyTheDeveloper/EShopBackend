using MINT.EShop.Business.DTOs.Identity;

namespace MINT.EShop.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse?> VerifyAsync(VerifyRequest request);
        Task<UserResponse?> UpdateDataAsync(Guid id, UpdateUserDataRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> PromoteToManagerAsync(Guid id);
        Task<bool> DemoteToClientAsync(Guid id);
    }
}
