using MINT.EShop.Business.Interfaces;
using MINT.EShop.Core.Entities;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _repo.GetUserByIdAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            user.Id = Guid.NewGuid();
            await _repo.AddUserAsync(user);
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingUser = await _repo.GetUserByIdAsync(user.Id);
            if (existingUser == null)
                return null;
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            await _repo.UpdateUserAsync(existingUser);
            return existingUser;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingUser = await _repo.GetUserByIdAsync(id);
            if (existingUser == null)
                return false;
            await _repo.DeleteUserAsync(id);
            return true;
        }
    }
}
