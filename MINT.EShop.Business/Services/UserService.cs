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
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await unitOfWork.Users.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            user.Id = Guid.NewGuid();
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.CompleteAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingUser = await unitOfWork.Users.GetByIdAsync(user.Id);
            if (existingUser == null)
                return null;
            unitOfWork.Users.Update(existingUser);
            await unitOfWork.CompleteAsync();  
            return existingUser;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingUser = await unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
                return false;
            unitOfWork.Users.Delete(existingUser);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
