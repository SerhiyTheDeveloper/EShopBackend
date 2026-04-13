using MINT.EShop.Business.DTOs.Identity;

namespace MINT.EShop.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<UserResponse> CreateAsync(RegisterRequest request);
        Task<UserResponse?> UpdateDataAsync(Guid id, UpdateUserDataRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> PromoteToManagerAsync(Guid id);
        Task<bool> DemoteToClientAsync(Guid id);
    }
}
