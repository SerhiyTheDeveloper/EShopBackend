using MINT.EShop.Business.DTOs.Identity;
using MINT.EShop.Core.Entities.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<UserResponse> CreateAsync(RegisterRequest request);
        Task<UserResponse?> UpdateDataAsync(Guid id, UpdateUserDataRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
