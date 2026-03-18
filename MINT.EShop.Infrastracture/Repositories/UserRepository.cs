using Microsoft.EntityFrameworkCore;
using MINT.EShop.Core.Entities.UserData;
using MINT.EShop.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINT.EShop.Infrastracture.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }
    }
}
