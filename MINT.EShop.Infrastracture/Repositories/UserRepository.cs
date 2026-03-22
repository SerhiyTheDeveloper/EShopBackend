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
            return await _dbContext.Users
                .Include(u => u.Credential)
                .Include(u => u.Sessions)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _dbContext.Users
                .Include(u => u.Credential)
                .Include(u => u.Sessions)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbContext.Users
                .Include(u => u.Credential)
                .Include(u => u.Sessions)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public void AddSession(UserSession session)
        {
            dbContext.Set<UserSession>().Add(session);
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }
    }
}
